namespace Waifustasia.Data.User;

public partial class User
{
    public class UserWaifuFollow
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string WaifuId { get; set; }
        public Waifu.Waifu Waifu { get; set; }
    }
}