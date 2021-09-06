using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text;


namespace TaskManager.Pages
{
    public class GroupsModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string button)
        {
            if(button == "addGroup")
            {
                return RedirectToPage("CreateGroup");
            }
            int id = Convert.ToInt32(button);
            HttpContext.Session.SetInt32("GroupId", id);
            //HttpContext.Session.SetString("GroupId", button);
            return RedirectToPage("GroupPage");
        }
    }
}
