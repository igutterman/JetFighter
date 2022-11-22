using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRGame.Pages
{
    //[IgnoreAntiforgeryToken(Order = 1001)]
    public class RoomModel : PageModel
    {

        public string Name { get; set; } = string.Empty;

        


        public ActionResult OnGet()
        {
            if (Name is not null && Name != string.Empty)
                return Page();
            else
                return RedirectToPage("/Error");
        }

        public void OnPost()
        {
            Console.WriteLine("/Room Post request");

            if (Request.Form is not null)
            {
                if (Request.Form["Name"][0] is not null)
                {
                    var a = Request.Form["Name"][0];
                    if (a is not null)
                        Name = a;
                }
                    
            }
            

 
        }
    }
}
