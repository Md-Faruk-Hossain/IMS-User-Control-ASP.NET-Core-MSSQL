using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using spa_core_final_project.Data;

namespace spa_core_final_project.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string userrole)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(userrole))
            {
                if (await roleManager.RoleExistsAsync(userrole))
                {
                    msg = "Role [" + userrole + "] already exists !!!";
                }
                else
                {
                    IdentityRole r = new IdentityRole(userrole);
                    await roleManager.CreateAsync(r);
                    msg = "Role [" + userrole + "] has been created successfully !!!";
                }

            }
            else
            {
                msg = "Please enter a valid role name !!!";
            }
            ViewBag.msg = msg;
            return View("Index");
        }
        public IActionResult AssignRole()
        {
            ViewBag.users = userManager.Users;
            ViewBag.roles = roleManager.Roles;
            ViewBag.msg = TempData["msg"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userdata, string roledata)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(userdata) && !string.IsNullOrEmpty(roledata))
            {
                ApplicationUser u = await userManager.FindByEmailAsync(userdata);
                if (u != null)
                {
                    if (await roleManager.RoleExistsAsync(roledata))
                    {
                        if (await userManager.IsInRoleAsync(u, roledata))
                        {
                            msg = "Role [" + roledata + "] already assigned to " + userdata + " !!!";
                        }
                        else
                        {
                            await userManager.AddToRoleAsync(u, roledata);
                            msg = "Role [" + roledata + "] has been assigned to " + userdata + " successfully !!!";
                        }
                    }
                    else
                    {
                        await userManager.GetUsersInRoleAsync(roledata);
                        msg = "Role [" + roledata + "] doesn't exists !!!";
                    }
                }
                else
                {
                    msg = userdata + "Not Found !!!";
                }

            }
            else
            {
                msg = "Please select a valid user and role";
            }
            ViewBag.users = userManager.Users;
            ViewBag.roles = roleManager.Roles;
            ViewBag.msg = msg;
            return View("AssignRole");
        }
    }
}
