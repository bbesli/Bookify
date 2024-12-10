using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

public class UsersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}