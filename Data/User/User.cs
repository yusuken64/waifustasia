using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Waifustasia.Data.User;
[Table("AspNetUsers")]
public partial class User : IdentityUser
{
    public User()
    {
        FollowedWaifus = new List<UserWaifuFollow>();
    }
    public string Password { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Role { get; set; } = "User";

	public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (nameof(Id), Id),
        new (ClaimTypes.Name, UserName),
        new (ClaimTypes.Hash, Password),
        new (nameof(DisplayName), DisplayName),
        new (ClaimTypes.Role, Role)
    }, "Waifustasia"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Id = principal.FindFirstValue(nameof(Id)),
        UserName = principal.FindFirstValue(ClaimTypes.Name),
        Password = principal.FindFirstValue(ClaimTypes.Hash),
        DisplayName = principal.FindFirstValue(nameof(DisplayName)),
        Role = principal.FindFirstValue(ClaimTypes.Role),
    };

    public List<Waifu.Waifu> CreatedWaifus { get; set; } = new List<Waifu.Waifu>();
    public int Coin { get; set; }
    public int Rizz { get; set; }
    public List<Memory.Memory> LikedMemories { get; set; }
    public ICollection<UserWaifuFollow> FollowedWaifus { get; set; }
}