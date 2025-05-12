using Microsoft.AspNetCore.Mvc;

namespace CardGamesSolution.Server.UserAccount
{
    public interface ILoginManager
    {
        IActionResult Login(UserDto user);
        IActionResult Register(UserDto user);
    }
}
