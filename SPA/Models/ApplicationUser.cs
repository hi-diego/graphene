using Graphene.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace SPA.Models;

public class ApplicationUser : GrapheneIdentityUser
{
    /// <summary>
    /// 
    /// </summary>
    public string TESTING { get; set; } = "TESTING";
}
