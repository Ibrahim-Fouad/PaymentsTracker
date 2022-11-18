using Microsoft.AspNetCore.Mvc;

namespace PaymentsTracker.API.Controllers;

public class AuthController : BaseController
{
    // GET
    [HttpGet]
    public bool CheckAuth()
    {
        return true;
    }
}