using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRChat.Pages
{
    //[IgnoreAntiforgeryToken(Order = 1001)]
    public class RoomModel : PageModel
    {

        public string Name { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            Console.WriteLine("/Room Post request");
            var formname = Request.Form["Name"];
            Name = formname;
        }
    }
}
