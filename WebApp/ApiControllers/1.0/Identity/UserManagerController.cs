using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
        /// Get registered AppUsers with roles.
        /// </summary>
        /// <returns>Collection of AppUsers.</returns>
        /// <response code="200">The AppUsers were successfully retrieved.</response>
        /// <response code="403">Not authorized to see data.</response>
        /// <response code="404">The AppUser object type does not exist.</response>
        [ProducesResponseType( typeof( IEnumerable<AppUser> ), 200 )]
        [ProducesResponseType( 404 )]
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
                    //FirstLastName = user.FirstLastName,
                    Roles = userEmailsWithRoles[user.Email]
                }).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = _roleManager.Roles.AsQueryable();

            return await roles.Select(role => new RoleDto
            {
                RoleName = role.Name
            }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateUser(UserCreateDto model)
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

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateRole(RoleDto model)
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
        
        [HttpPost]
        public async Task<ActionResult<AppUser>> DeleteRole(RoleDto roleDTO)
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
        
        [HttpPost]
        public async Task<ActionResult<AppUser>> AddRoleToUser(UserRoleDto userRoleDTO)
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
        

    }
}