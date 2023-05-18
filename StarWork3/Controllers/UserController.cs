using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StarWork3.Controllers
{
    [Authorize]
    [Route("/user")]
    public class UserController : Controller
    {

    }
}
