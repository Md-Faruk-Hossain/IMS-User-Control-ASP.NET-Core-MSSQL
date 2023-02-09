using Microsoft.AspNetCore.Identity;

namespace spa_core_final_project.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = null!;
        public string CellPhone { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}
