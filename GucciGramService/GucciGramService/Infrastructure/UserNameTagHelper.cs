using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GucciGramService.Models;
using Microsoft.AspNetCore.Identity;

namespace GucciGramService.Infrastructure
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("a", Attributes = "user-name")]
    public class UserNameTagHelper : TagHelper
    {
        private UserManager<User> userManager;

        public UserNameTagHelper(UserManager<User> usermgr)
        {
            userManager = usermgr;
        }

        [HtmlAttributeName("user-name")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            User user = await userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                output.Content.SetContent(user.UserName);
            }
        }
    }
}
