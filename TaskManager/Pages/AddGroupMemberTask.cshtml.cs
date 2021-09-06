using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TaskManagerDatabase;

namespace TaskManager.Pages
{
    public class AddGroupMemberTaskModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string button, string taskName, string taskDescription, string date)
        {
            if (button == "cancel")
            {
                return RedirectToPage("GroupPage");
            }
            if (taskName == null || taskDescription == null || date == null)
            {
                TempData["error"] = "Please fill out all fields";
                return new PageResult();
            }
           
            DateTimeOffset formattedDate = DateTime.Parse(date);
            var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
            if (button == "add")
            {
                int memberId = HttpContext.Session.GetInt32("GroupMemberId") ?? default(int);
                repo.CreateGroupMemberTask( HttpContext.Session.GetInt32("GroupId") ?? default(int), memberId, taskName, taskDescription, formattedDate);
                return RedirectToPage("GroupPage");
            }

            return new PageResult();
        }
    }
}
