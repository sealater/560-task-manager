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
    public class GroupPageModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string button)
        {
            if(button == "addMember")
            {
                return RedirectToPage("AddGroupMember");
            }
            else if(button == "addGroupTask")
            {
                return RedirectToPage("AddGroupTask");
            }
            else if (button[0] == 'c')
            {
                var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                string str = button.Substring(9);
                repo.CompleteTask(Int32.Parse(str));
            }
            else if (button[0] == 'd')
            {
                var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                repo.RemoveTask(Int32.Parse(button.Substring(7)));
            }
            else if (button == "addIndividualTask")
            {
                HttpContext.Session.SetInt32("GroupMemberId", HttpContext.Session.GetInt32("UserId") ?? default(int));
                return RedirectToPage("AddGroupMemberTask");
                
            }
            else if (button.Substring(0, 18) == "addGroupMemberTask")
            {
                int id = Int32.Parse(button.Substring(19));
                HttpContext.Session.SetInt32("GroupMemberId", id);
                return RedirectToPage("AddGroupMemberTask");
            }

            return new PageResult();
        }
    }

}
