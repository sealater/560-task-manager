using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerDatabase.Models;
using TaskManagerDatabase;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Pages
{
    public class AddGroupMemberModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string button, string userName)
        {
            if (button == "cancel")
            {
                return RedirectToPage("GroupPage");
            }
            if (userName == null)
            {
                TempData["error"] = "Please enter username";
                return new PageResult();
            }
            if (button == "add")
            {
                bool flag = false;
                User user = new SqlUserRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;").GetUserByUsername(userName);
                int groupId = HttpContext.Session.GetInt32("GroupId") ?? default(int);

                if (user != null)
                {
                    IReadOnlyList<Group> test = new SqlGroupRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;").GetGroupsByUserId(user.UserId);
                    foreach (Group g in test)
                    {
                        if (g.GroupId == groupId)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        var repo = new SqlGroupRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                        repo.AddGroupMember(groupId, user.UserId);
                        return RedirectToPage("GroupPage");
                    }

                }
                TempData["error"] = "Could not add user";
            }
            

            return new PageResult();
        }
    }
}
