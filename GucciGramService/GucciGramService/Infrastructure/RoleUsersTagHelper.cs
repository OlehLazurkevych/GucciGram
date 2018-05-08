using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GucciGramService.Models;

namespace GucciGramService.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUsersTagHelper(UserManager<User> usermgr, RoleManager<IdentityRole> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }

        [HtmlAttributeName("identity-role")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> roles = new List<string>();
            User user = await userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                foreach (var role in roleManager.Roles)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        roles.Add(role.Name); /// ADD COMMENTS SOMEWERE HERE
                    }
                }
            }
            output.Content.SetContent(roles.Count == 0 ? "No Roles" : string.Join(", ", roles));
        }
    }
}
