using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Add custom properties to the user model
    public string FullName { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int phonenumber { get; set; }
}

