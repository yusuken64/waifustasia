namespace Waifustasia.Data
{
    public static class PrepareDb
	{
		public static void PreparePopulation(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<WaifustasiaDbContext>());
			}
		}

		private static void SeedData(WaifustasiaDbContext? context)
		{
			if (context == null) { throw new ArgumentNullException(nameof(context)); };

			//context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			if (context.Users.Any()) 
			{
				Console.WriteLine("Data already exists");
				return;
			}

			Console.WriteLine("Seeding data...");
			context.Users.Add(new User.User()
			{
				UserName = "Email@Email.com",
				Email = "Email@Email.com",
				Password = "Password1!",
				DisplayName= "Rizzer",
				Role = "Admin",
			});
			context.Users.Add(new User.User()
			{
				UserName = "Email2@Email.com",
				Email = "Email2@Email.com",
				Password = "Password2@",
				DisplayName = "Rizzer",
				Role = "SuperUser",
			});
			context.SaveChanges();

			var testUser = context.Users.First();
			context.Waifus.Add(new Waifu.Waifu()
			{
				Id = "3",
				Name = "Waifu 3",
				Creator = testUser,
				ImageUrl = "https://i.imgur.com/Hjd1KjX_d.webp?maxwidth=520&shape=thumb&fidelity=high",
				PersonalityPrompt = "2B,[a] full name YoRHa No. 2 Type B,[b] is a fictional android from the 2017 video game Nier: Automata, a spin-off of the Drakengard series developed by PlatinumGames and published by Square Enix. One of the game's three protagonists, 2B is a soldier for YoRHa, an android task force fighting a proxy war with alien-created Machine Lifeforms. Her story arc focuses on her backstory within YoRHa, and her relationship with her partner 9S, a reconnaissance android. She is also featured in related media, such as the anime Nier: Automata Ver1.1a.",
				ProfileDescription = "I'm from the future",
				Memories = new List<Data.Memory.Memory>()
			{
				new Data.Memory.Memory()
				{
					MemoryUser = testUser,
					MemoryUserId= testUser.Id,
					GenerationPrompt = "Test Prompt",
					MemoryImageUrl = @"\Images\3e573534-c617-4e55-8d54-69a198e3c8f1.jpg",
					MemoryDescription = "Just when I thought sunsets couldn't get any more breathtaking, he came into the picture. 🌅✨ An evening filled with laughter, good vibes, and the most beautiful views. Feeling grateful for moments like these! #DateNightMagic #MakingMemories",
					CreationDate = DateTime.UtcNow - TimeSpan.FromDays(1)
				},
				new Data.Memory.Memory()
				{
					MemoryUser = testUser,
					MemoryUserId= testUser.Id,
					GenerationPrompt = "Test Prompt2",
					MemoryImageUrl = @"\Images\7eb3237b-e65d-4a22-a1a2-f05a34812b04.jpg",
					MemoryDescription = "\"Sometimes, you just click with someone in unexpected ways. 💫🌹 Our date was like a scene from a romantic movie—good food, great company, and endless conversations. Here's to more moments that make you smile from ear to ear! #PerfectDate #HeartFull\"",
					CreationDate = DateTime.UtcNow - TimeSpan.FromDays(1)
				}
			}
			});
			context.Waifus.Add(new Waifu.Waifu()
			{
				Id = "4",
				Name = "Waifu 4",
				Creator = testUser,
				ImageUrl = "https://i.imgur.com/i3CXeGe_d.webp?maxwidth=520&shape=thumb&fidelity=high",
				PersonalityPrompt = "2B,[a] full name YoRHa No. 2 Type B,[b] is a fictional android from the 2017 video game Nier: Automata, a spin-off of the Drakengard series developed by PlatinumGames and published by Square Enix. One of the game's three protagonists, 2B is a soldier for YoRHa, an android task force fighting a proxy war with alien-created Machine Lifeforms. Her story arc focuses on her backstory within YoRHa, and her relationship with her partner 9S, a reconnaissance android. She is also featured in related media, such as the anime Nier: Automata Ver1.1a.",
				ProfileDescription = "I'm just a girl",
				Memories = new List<Data.Memory.Memory>()
			{
				new Data.Memory.Memory()
				{
					MemoryUser = testUser,
					MemoryUserId= testUser.Id,
					GenerationPrompt = "Test Prompt",
					MemoryImageUrl = @"\Images\aa76784a-220b-45e2-b50d-4e8518b4eb25.jpg",
					MemoryDescription = "Test Description",
					CreationDate = DateTime.UtcNow - TimeSpan.FromDays(1)
				}
			}
			});

			context.SaveChanges();
		}
	}
}
