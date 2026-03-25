using System.Text.Json;
using HardwareStore.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HardwareStore.Web.Mvc.Components;

public class CategoriesViewComponent : ViewComponent
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly IWebHostEnvironment hostingEnvironment;

    public CategoriesViewComponent(IWebHostEnvironment hostingEnvironment)
    {
        this.hostingEnvironment = hostingEnvironment;
    }

    public IViewComponentResult Invoke()
    {
        var path = Path.Combine(this.hostingEnvironment.WebRootPath, "data", "categories-nav.json");
        var json = File.ReadAllText(path);
        var categories = JsonSerializer.Deserialize<CategoriesNavModel>(json, JsonOptions)
            ?? throw new InvalidOperationException("Invalid categories navigation JSON.");

        return View(categories);
    }
}
