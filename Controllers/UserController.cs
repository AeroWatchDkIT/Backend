using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;
using PalletSyncApi.Classes;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

       

    }
}
