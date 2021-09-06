using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TaskManagerDatabase;
using TaskManagerDatabase.Models;

namespace TaskManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost(string button)
        {
            //string str = button.Substring(9);
            if (button == "addTask")
            {
                return RedirectToPage("CreateUserTask");
            }
            else if (button[0]== 'c')
            {
                var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                string str = button.Substring(9);
                repo.CompleteTask(Int32.Parse(str));
            }
            else if(button.Substring(0, 6) == "delete")
            {
                var repo = new SqlTaskRepository(@"Server=.\SQLEXPRESS; Database=model; Integrated Security=SSPI;");
                repo.RemoveTask(Int32.Parse(button.Substring(7)));
            }
            else if(button == "addTask")
            {
                return RedirectToPage("CreateUserTask");
            }

            return new PageResult();

        }
    }
}
