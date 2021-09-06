using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerDatabase;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Pages
{
    public class AddGroupTaskModel : PageModel
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
            if (button == "add")
            {
                DateTimeOffset formattedDate = DateTimeOffset.Parse(date);
                var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                int groupId = HttpContext.Session.GetInt32("GroupId") ?? default(int);
                repo.CreateGroupTask(groupId, taskName, taskDescription, formattedDate);
            }
            return RedirectToPage("GroupPage");
        }
    }
}
