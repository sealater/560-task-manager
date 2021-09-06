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
    public class CreateGroupModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string button, string groupName)
        {
            if (button == "cancel")
            {
                return RedirectToPage("Groups");
            }
            if (groupName == null)
            {
                TempData["error"] = "Please enter group name";
                return new PageResult();
            }
            if (button == "create")
            {
                var repo = new SqlGroupRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                var check = repo.GetGroupByGroupName(groupName);
                if (check == null)
                {
                    var group = repo.CreateGroup(groupName, HttpContext.Session.GetInt32("UserId") ?? default(int));

                    repo.AddGroupMember(group.GroupId, HttpContext.Session.GetInt32("UserId") ?? default(int));
                    return RedirectToPage("Groups");

                }
                else
                {
                    TempData["error"] = "Group Name already exists";
                    return new PageResult();
                }

                
            }

      
            return new PageResult();
        }
    }
}
