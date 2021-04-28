using System.Threading.Tasks;
using API.DTO.v1.Identity;
using Contracts.BLL.App;
using Domain;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

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
        [ProducesResponseType(200, Type = typeof(JwtResponseDto))]
        [ProducesResponseType(404, Type = typeof(MessageDto))]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(MessageDto))]
        [ProducesResponseType(404, Type = typeof(MessageDto))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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