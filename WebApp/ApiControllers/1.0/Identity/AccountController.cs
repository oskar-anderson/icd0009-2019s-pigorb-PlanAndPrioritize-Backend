using System.Threading.Tasks;
using API.DTO.v1.Identity;
using Contracts.BLL.App;
using Domain;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp.ApiControllers._1._0.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configurations</param>
        /// <param name="userManager">User Manager</param>
        /// <param name="signInManager">Sign In Manager</param>
        /// <param name="logger">Logger</param>
        /// <param name="roleManager">Role Manager</param>
        /// <param name="bll">Services container</param>
        public AccountController(IConfiguration configuration, UserManager<AppUser> userManager,
            ILogger<AccountController> logger, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager,
            IAppBLL bll)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _bll = bll;
        }

        /// <summary>
        /// Endpoint for user log-in (jwt generation)
        /// </summary>
        /// <param name="model">login data</param>
        /// <returns>Status Code (and new object with jwt value and status message in case of success)</returns>
        /// <response code="200">Successful login.</response>
        /// <response code="404">User with this email was not found.</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageDto))]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogInformation($"Web-Api login. User {model.Email} not found!");
                return NotFound(new MessageDto("User not found!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                var jwt = IdentityExtensions.GenerateJWT(claimsPrincipal.Claims,
                    _configuration["JWT:SigningKey"],
                    _configuration["JWT:Issuer"],
                    _configuration.GetValue<int>("JWT:ExpirationInDays")
                );

                var jwtResponse = new JwtResponseDto()
                    {Token = jwt, Status = $"User {user.Email} logged in.", RequirePasswordChange = false};

                var isFirstLogin = false;
                if (_userManager.GetLoginsAsync(user).Result.Count == 0)
                {
                    _logger.LogInformation($"Users first login.");
                    isFirstLogin = true;
                }

                jwtResponse.RequirePasswordChange = isFirstLogin;

                _logger.LogInformation($"WebApi login. User {user.Email} logged in.");
                return Ok(jwtResponse);
            }

            _logger.LogInformation($"Web-Api login. User {model.Email} attempted to log-in with bad password!");
            return NotFound(new MessageDto("User not found!"));
        }

        /// <summary>
        /// Endpoint for user registration and immediate log-in (jwt generation) 
        /// </summary>
        /// <param name="model">user data</param>
        /// <returns>Status Code (and IdentityResult in case of success)</returns>
        /// <response code="200">User was successfully registered.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">User with this email already exists.</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageDto))]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDto model)
        {
            var role = await _roleManager.FindByNameAsync("User");

            if (role == null)
            {
                role = new AppRole {Name = "User"};
                var roleCreationResult = await _roleManager.CreateAsync(role);

                if (!roleCreationResult.Succeeded)
                {
                    _logger.LogInformation("Role creation failed!");
                    return BadRequest(new MessageDto("Role creation failed!"));
                }
            }

            var userName = model.Email;
            var passWord = model.Password;
            var firstName = model.FirstName;
            var lastName = model.LastName;

            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                _logger.LogInformation($"Username {model.Email} already exists! Choose another one");
                return NotFound(new MessageDto("User with that username already registered!"));
            }

            user = new AppUser {Email = userName, UserName = userName, FirstName = firstName, LastName = lastName};

            var result = await _userManager.CreateAsync(user, passWord);

            if (!result.Succeeded)
            {
                _logger.LogInformation("User creation failed!");
                return BadRequest(new MessageDto("User creation failed!"));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                _logger.LogInformation("Role adding failed!");
                return BadRequest(new MessageDto("Role adding failed!"));
            }

            _logger.LogInformation($"User {user.Email} created a new account with password.");


            var createdUser = await _userManager.FindByEmailAsync(user.Email);
            if (createdUser != null)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(createdUser);
                var jwt = IdentityExtensions.GenerateJWT(
                    claimsPrincipal.Claims,
                    _configuration["JWT:SigningKey"],
                    _configuration["JWT:Issuer"],
                    _configuration.GetValue<int>("JWT:ExpirationInDays")
                );
                _logger.LogInformation($"WebApi register. User {createdUser.Email} logged in.");
                return Ok(new JwtResponseDto()
                    {Token = jwt, Status = $"User {createdUser.Email} created and logged in."});
            }

            _logger.LogInformation($"User {user.Email} not found after creation!");
            return BadRequest(new MessageDto("User not found after creation!"));
        }

        /// <summary>
        /// Endpoint for mandatory user password change in case of first login 
        /// </summary>
        /// <param name="model">user data with old and new password</param>
        /// <returns>Status Code (and IdentityResult in case of success)</returns>
        /// <response code="200">Password was successfully changed.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">User for this email was not found.</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MessageDto))]
        public async Task<ActionResult<string>> ChangePassword([FromBody] PasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogInformation($"Web-Api password change attempt. User {model.Email} not found!");
                return NotFound(new MessageDto("User not found!"));
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Password change for user {model.Email}.");
                await _userManager.AddLoginAsync(user, new UserLoginInfo("PlanAndPrioritize", user.Email, user.Email));
                return Ok();
            }

            _logger.LogInformation($"Password change for user {user.Email} failed!");
            return BadRequest(new MessageDto("Password change failed!"));
        }
    }
}