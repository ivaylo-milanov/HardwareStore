namespace HardwareStore.Web.Api.Controllers
{
    using HardwareStore.Web.Api.Extensions;
    using HardwareStore.Common;
    using HardwareStore.Core.ViewModels.Profile;
    using HardwareStore.Infrastructure.Models;
    using HardwareStore.Web.Api.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<Customer> userManager;
        private readonly ILogger<ProfileController> logger;

        public ProfileController(UserManager<Customer> userManager, ILogger<ProfileController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileViewModel>> Get()
        {
            var customer = await this.userManager.FindByIdAsync(this.User.GetUserId());
            if (customer == null)
            {
                this.logger.LogError(LogMessages.ProfileUnknownUser, this.User.GetUserId());
                return this.NotFound();
            }

            return this.Ok(new ProfileViewModel
            {
                FullName = $"{customer.FirstName} {customer.LastName}",
                City = customer.City,
                Address = customer.Address,
                Email = customer.Email ?? string.Empty,
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProfileUpdateRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            var customer = await this.userManager.FindByIdAsync(this.User.GetUserId());
            if (customer == null)
            {
                this.logger.LogError(LogMessages.ProfileUnknownUser, this.User.GetUserId());
                return this.NotFound();
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.PhoneNumber = request.Phone;
            customer.City = request.City;
            customer.Area = request.Area;
            customer.Address = request.Address;

            var result = await this.userManager.UpdateAsync(customer);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(error.Code, error.Description);
                }

                return this.ValidationProblem(this.ModelState);
            }

            return this.NoContent();
        }
    }
}
