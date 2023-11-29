using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waifustasia.Data.Memory
{
	public class Memory
	{
		[Key]
		public string Id { get; set; }
		public string GenerationPrompt { get; set; }
		public string MemoryImageUrl { get; set; }
		public string MemoryDescription { get; set; }
		public DateTime CreationDate { get; set; }

		//statistics
		public int Likes { get; set; }
		public int Views { get; set; }

		public string MemoryUserId;
		[ForeignKey("MemoryUserId")]
		public User.User MemoryUser { get; set; }

		public string WaifuId;
		[ForeignKey("WaifuId")]
		public Waifu.Waifu Waifu { get; set; }  // Navigation property
	}
}
