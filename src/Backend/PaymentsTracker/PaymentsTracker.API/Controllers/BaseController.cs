using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentsTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
}