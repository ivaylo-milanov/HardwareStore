namespace HardwareStore.Components
{
    using HardwareStore.Core.ViewModels.User;
    using Microsoft.AspNetCore.Mvc;

    public class LoginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            LoginFormModel model = new LoginFormModel();

            return View(model);
        }
    }
}
