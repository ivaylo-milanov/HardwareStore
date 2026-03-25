namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Core.ViewModels.User;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Web.Api.Models;
    using HardwareStore.Web.Api.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;
        private readonly IJwtTokenService jwtTokenService;
        private readonly ILogger<AuthController> logger;

        public AuthController(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IJwtTokenService jwtTokenService,
            ILogger<AuthController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtTokenService = jwtTokenService;
            this.logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthTokenResponse>> Login([FromBody] LoginFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return this.Unauthorized();
            }

            var result = await this.signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return this.Unauthorized();
            }

            var token = this.jwtTokenService.CreateToken(user);
            return this.Ok(new AuthTokenResponse
            {
                AccessToken = token,
                ExpiresIn = this.jwtTokenService.ExpiresInSeconds,
                Email = user.Email ?? string.Empty,
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthTokenResponse>> Register([FromBody] RegisterFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            var customer = new Customer
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                City = model.City,
                Area = model.Area,
                Address = model.Address,
            };

            var result = await this.userManager.CreateAsync(customer, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(error.Code, error.Description);
                }

                return this.ValidationProblem(this.ModelState);
            }

            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                this.logger.LogError("Registered user not found after creation: {Email}", model.Email);
                return this.Problem(statusCode: StatusCodes.Status500InternalServerError);
            }

            var token = this.jwtTokenService.CreateToken(user);
            var response = new AuthTokenResponse
            {
                AccessToken = token,
                ExpiresIn = this.jwtTokenService.ExpiresInSeconds,
                Email = user.Email ?? string.Empty,
            };
            return this.StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return this.Ok();
        }
    }
}
