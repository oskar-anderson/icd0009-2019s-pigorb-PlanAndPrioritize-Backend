using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1.Identity;
using API.DTO.v1.UserManager;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.ApiControllers._1._0.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UserManagerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">User Manager</param>
        /// <param name="roleManager">Role Manager</param>
        /// <param name="logger">Logger</param>
        public UserManagerController(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Get registered users with roles.
        /// </summary>
        /// <returns>Collection of users.</returns>
        /// <response code="200">The users were successfully retrieved.</response>
        /// <response code="403">Not authorized to see data.</response>
        /// <response code="404">The AppUser object type does not exist.</response>
        [ProducesResponseType(typeof(IEnumerable<UserIndexDto>), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserIndexDto>>> GetUsers()
        {
            var userEmailsWithRoles = new Dictionary<string, string>();

            foreach (var user in _userManager.Users)
            {
                var roles = new List<string>();
                foreach (var role in _roleManager.Roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        roles.Add(role.Name);
                    }
                }

                var rolesAsString = string.Join(", ", roles);
                userEmailsWithRoles[user.Email] = rolesAsString;
            }

            var users = _userManager.Users.AsQueryable();

            return await users.Select(user => new UserIndexDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstLastName = user.FirstName + " " + user.LastName,
                Roles = userEmailsWithRoles[user.Email]
            }).ToListAsync();
        }

        /// <summary>
        /// Get roles
        /// </summary>
        /// <returns>Collection of roles</returns>
        /// <response code="200">The roles were successfully retrieved.</response>
        /// <response code="403">Not authorized to see data.</response>
        /// <response code="404">The AppRole object type does not exist.</response>
        [ProducesResponseType(typeof(IEnumerable<RoleDto>), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = _roleManager.Roles.AsQueryable();

            return await roles.Select(role => new RoleDto
            {
                RoleName = role.Name
            }).ToListAsync();
        }

        /// <summary>
        /// Create new user with role
        /// </summary>
        /// <param name="model">New user data</param>
        /// <returns>Action result</returns>
        /// <response code="200">The user was successfully created.</response>
        /// <response code="403">Not authorized to perform action.</response>
        /// <response code="404">Role was not found.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserCreateDto model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);

            if (role == null)
            {
                _logger.LogInformation("Role not found!");
                return StatusCode(404);
            }

            var userName = model.Email;
            var passWord = model.Password;
            var firstName = model.FirstName;
            var lastName = model.LastName;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new AppUser {Email = userName, UserName = userName, FirstName = firstName, LastName = lastName};

                var result = await _userManager.CreateAsync(user, passWord);

                if (!result.Succeeded)
                {
                    _logger.LogInformation("User creation failed!");
                    return StatusCode(403);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    _logger.LogInformation("Role adding failed!");
                    return StatusCode(403);
                }

                _logger.LogInformation($"User {model.Email} created!");
                return Ok(roleResult);
            }

            _logger.LogInformation($"Username {model.Email} already exists! Choose another one");
            return StatusCode(403);
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="model">Role data</param>
        /// <returns>Action result</returns>
        /// <response code="200">The role was successfully created.</response>
        /// <response code="403">Not authorized to perform action.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleDto model)
        {
            var role = new AppRole {Name = model.RoleName};

            if (role == null)
            {
                _logger.LogInformation("Role creation failed!");
                return StatusCode(403);
            }

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Role creation failed!");
                return StatusCode(403);
            }

            _logger.LogInformation($"Role {model.RoleName} created!");
            return Ok(result);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userDeleteDTO">User data</param>
        /// <returns>User deleted</returns>
        /// <response code="200">User was successfully deleted.</response>
        /// <response code="403">Not authorized to perform action.</response>
        /// <response code="404">User not found.</response>
        /// <response code="405">User deletion failed.</response>
        [ProducesResponseType(typeof(AppUser), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AppUser>> DeleteUser(UserDeleteDto userDeleteDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDeleteDTO.Email);

            foreach (var appUser in await _userManager.Users.ToListAsync())
            {
                if (appUser.Id == user.Id)
                {
                    foreach (var role in await _roleManager.Roles.ToListAsync())
                    {
                        if (await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                    }
                }
            }

            if (user == null)
            {
                _logger.LogInformation("User not found!");
                return StatusCode(404);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogInformation("User deleting failed!");
                return StatusCode(405);
            }

            return Ok(user);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="roleDTO">Role data</param>
        /// <returns>Deleted role</returns>
        /// <response code="200">Role was successfully deleted.</response>
        /// <response code="403">Not authorized to perform action.</response>
        /// <response code="404">Role not found.</response>
        /// <response code="405">Role deletion failed.</response>
        [ProducesResponseType(typeof(AppRole), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AppRole>> DeleteRole(RoleDto roleDTO)
        {
            if (roleDTO.RoleName == null)
            {
                _logger.LogInformation("Role not found!");
                return StatusCode(404);
            }

            var role = await _roleManager.FindByNameAsync(roleDTO.RoleName);

            if (role == null)
            {
                _logger.LogInformation("Role not found!");
                return StatusCode(404);
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Role deleting failed!");
                return StatusCode(405);
            }

            return Ok(role);
        }

        /// <summary>
        /// Add role to user
        /// </summary>
        /// <param name="userRoleDTO">User and role data</param>
        /// <returns>Action result</returns>
        /// <response code="200">Role was successfully added to user.</response>
        /// <response code="403">Not authorized to perform action.</response>
        /// <response code="404">User or role not found.</response>
        /// <response code="405">Role adding to user failed.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddRoleToUser(UserRoleDto userRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDTO.Email);

            if (user == null)
            {
                _logger.LogInformation("User not found!");
                return StatusCode(404);
            }

            if (userRoleDTO.RoleName == null || await _roleManager.FindByNameAsync(userRoleDTO.RoleName) == null)
            {
                _logger.LogInformation("Role not found!");
                return StatusCode(404);
            }

            var result = await _userManager.AddToRoleAsync(user, userRoleDTO.RoleName);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Role can't be added!");
                return StatusCode(405);
            }

            _logger.LogInformation($"Role {userRoleDTO.RoleName} added!");
            return Ok(result);
        }

        /// <summary>
        /// Remove role from user
        /// </summary>
        /// <param name="userRoleDTO">User and role data</param>
        /// <returns>Action result</returns>
        /// <response code="200">Role was successfully removed from user.</response>
        /// <response code="403">Not authorized to perform action.</response>
        /// <response code="404">User or role not found.</response>
        /// <response code="405">Role removing from user failed.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(405)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AppUser>> RemoveRoleFromUser(UserRoleDto userRoleDTO)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDTO.Email);

            if (user == null)
            {
                _logger.LogInformation("User not found!");
                return StatusCode(404);
            }

            if (userRoleDTO.RoleName == null || await _roleManager.FindByNameAsync(userRoleDTO.RoleName) == null)
            {
                _logger.LogInformation("Role not found!");
                return StatusCode(404);
            }

            var result = await _userManager.RemoveFromRoleAsync(user, userRoleDTO.RoleName);

            if (!result.Succeeded)
            {
                _logger.LogInformation("Role can't be removed!");
                return StatusCode(405);
            }

            _logger.LogInformation($"Role {userRoleDTO.RoleName} remover!");
            return Ok(result);
        }
        
        /// <summary>
        /// Reset user password without knowing old password 
        /// </summary>
        /// <param name="model">User data with new password</param>
        /// <returns>Action result</returns>
        /// <response code="200">Password was successfully changed.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">User for this email was not found.</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogInformation($"Web-Api password reset attempt. User {model.Email} not found!");
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation($"Password reset for user {model.Email}.");
                await _userManager.RemoveLoginAsync(user, "PlanAndPrioritize", user.Email);
                return Ok();
            }

            _logger.LogInformation($"Password reset for user {user.Email} failed!");
            return BadRequest();
        }
    }
}