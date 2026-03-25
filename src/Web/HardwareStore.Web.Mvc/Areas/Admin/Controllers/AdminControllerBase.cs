namespace HardwareStore.Web.Mvc.Areas.Admin.Controllers
{
    using HardwareStore.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = AdminConstants.AdminRoleName)]
    public abstract class AdminControllerBase : Controller
    {
    }
}
