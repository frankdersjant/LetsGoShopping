using Microsoft.AspNet.Identity.EntityFramework;

namespace Data.OWIN
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
    }
}
