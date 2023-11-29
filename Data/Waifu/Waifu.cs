using System.ComponentModel.DataAnnotations;
using static Waifustasia.Data.User.User;

namespace Waifustasia.Data.Waifu;

public class Waifu
{
    public Waifu()
    {
        Followers = new List<UserWaifuFollow>();
    }

    [Key]
    public string Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    // User who created the Waifu
    public string UserId { get; set; } // Foreign key
    public User.User? Creator { get; set; } // Navigation property
    public int Score { get; set; }
    [Required(ErrorMessage = "Personality prompt is required")]
    public string? PersonalityPrompt { get; set; }
    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    public string? ImageUrl { get; set; }
    public List<Memory.Memory> Memories { get;set;}
    public string? ProfileDescription { get; set; }
    public ICollection<UserWaifuFollow> Followers { get; set; }
}
