namespace Waifustasia.Data.Chat
{
    public class Chat
    {
        public int ChatId { get; set; }
        public string? UserId { get; set; }
        public User.User? User { get; set; }
        public string? WaifuId { get; set; }
        public Waifu.Waifu? Waifu { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }

    public class Message
    {
        public int MessageId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }

        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public bool SentByUser { get; set; }
    }
}
