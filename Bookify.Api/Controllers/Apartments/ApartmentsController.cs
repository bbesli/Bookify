using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

public class ApartmentsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}