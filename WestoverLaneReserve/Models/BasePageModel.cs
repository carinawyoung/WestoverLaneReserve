using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestoverLaneReserve.Models;

public class BasePageModel : PageModel
{
    protected UserManager<CustomerApplicationUser> UserManager;

    public BasePageModel(UserManager<CustomerApplicationUser> userManager)
    {
        UserManager = userManager;
    }

    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }

    public async Task LoadUser()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user != null)
        {
            UserFirstName = user.FirstName;
            UserLastName = user.LastName;
            ViewData["UserFirstName"] = UserFirstName;
            ViewData["UserLastName"] = UserLastName;
        }
    }


    //If I were to user polymorphism I could change the code to this:
    //     public async Task LoadUser()
    // {
    //     var user = await UserManager.GetUserAsync(User);
    //     if (user != null)
    //     {
    //         UserFirstName = user.FirstName;
    //         ViewData["UserFirstName"] = UserFirstName;
    //     }
    // }
}
