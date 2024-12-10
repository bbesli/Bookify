using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookings;

public class BookingsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}