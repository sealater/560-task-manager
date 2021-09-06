using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagerDatabase.Models;
using TaskManagerDatabase;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace TaskManager.Pages
{
    public class LoginModel : PageModel
    {
        User _user;

        public void PopulateUser(string id)
        {
            _user = new SqlUserRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;").GetUserByUsername(id);
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string id, string password)
        {
            
            if (id != null && password != null)
            {
                PopulateUser(id);
                if (_user != null && _user.PasswordHash.Equals(password)) //FIX ME -- What if id or password is null?
                {
                    HttpContext.Session.SetInt32("UserId", _user.UserId);
                    return RedirectToPage("Dashboard");

                }
                TempData["error"] = "Failed to log in: Incorrect username or password";
                return new PageResult();
            }

            TempData["error"] = "Please enter both username and password.";
            return new PageResult();



        }

    }
}
