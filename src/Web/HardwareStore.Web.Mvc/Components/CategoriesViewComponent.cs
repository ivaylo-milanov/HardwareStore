using System;
using HardwareStore.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HardwareStore.Web.Mvc.Components;

public class CategoriesViewComponent : ViewComponent
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public CategoriesViewComponent(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public IViewComponentResult Invoke()
    {
        var path = Path.Combine(_hostingEnvironment.WebRootPath, "data", "categories-nav.json");
        var json = System.IO.File.ReadAllText(path);
        var categories = JsonConvert.DeserializeObject<CategoryModel>(json);

        return View(categories);
    }
}
