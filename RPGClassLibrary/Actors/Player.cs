using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;
using RPGClassLibrary.Items;
using System.Text.Json.Serialization;

namespace RPGClassLibrary.Actors
{
	public class Player : Entity
	{
		private static LoggingService log = new LoggingService(Sender.BACK); //not part of the player class

		public Player(string name, Role role, EntityStats stats, int[] statDist) : base(name, role)
		{
			Stats = stats;
			Money = 100;
			StatDistribution = statDist;
			AllocationPoints = 20;
			Inventory = new List<Item>();
			CurrentEffects = new List<Effect>();
		}

		public Player() { }
		public static Player PlayerFactory(string playerName, string roleName, string roleDesc = null)
		{
			try
			{
				bool running = true;
				while (running)
				{
					int points = 30;
					Console.WriteLine("There are 6 stats.");
					Console.WriteLine("HEALTH, MANA, STRENGTH, MAGIC STRENGTH, DEFENSE and DEXTERITY.");
					Console.WriteLine($"You have {points} points to allocate to these stats.");
					Console.WriteLine("[HEALTH] -- How much damage you can take in a battle before you fall.");
					Console.WriteLine("[MANA] -- Each spell will have a mana cost. The higher your mana, the more spells and the more powerful spells you can cast.");
					Console.WriteLine("[STRENGTH] -- Your physical strength, which influences how much damage you deal.");
					Console.WriteLine("[MAGIC STRENGTH] -- The strength of your magical attacks and support spells.");
					Console.WriteLine("[DEFENSE] -- Your defense towards physical and magical attacks. This reduces damage taken.");
					Console.WriteLine("[DEXTERITY] -- Your dexterity influences how fast you are and how easily you can dodge attacks and traps.");
					Console.WriteLine("Please format it like this. (0 0 0 0 0 0)");
					string input = Console.ReadLine();

					if (String.IsNullOrEmpty(input))
					{
						Console.WriteLine("Invalid input.");
						Console.ReadKey();
						Console.Clear();
						continue;
					}

					string[] scores = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					List<int> ints = new List<int>();

					foreach (string str in scores)
					{
						if (int.TryParse(str, out int result))
						{
							ints.Add(result);
						}
						else
						{
							Console.WriteLine($"{str} is an invalid input.");
						}
					}

					if (ints.Count < 6)
					{
						Console.WriteLine("You must enter a number for each stat!");
						continue;
					}

					int sum = ints.Sum();

					if (sum > points)
					{
						Console.WriteLine($"The sum of these points exceeds {points} points.");
						continue;
					}
					else if (sum < points)
					{
						Console.WriteLine($"You have {points - sum} missing points to allocate.");
						continue;
					}
					else
					{
						Console.WriteLine("Choose your specialization type.");
						Console.WriteLine("[BALANCED] -- No prioritization, all stats grow equally.");
						Console.WriteLine("[MAGIC] -- Prioiritzing growth in magic stats like Mana, Magic Strength and Dexterity. Suited for mages.");
						Console.WriteLine("[PHYSICAL] -- Prioritizing growth in physical stats like Health, Strength and Defense. Suited for knights and fighters.");
						Console.WriteLine("[HYBRID] -- Both physical and magic strength grow together, at a lower rate than their pure specialisation. Suited for highly versatile hybrid roles.");
						string growtype = Console.ReadLine().ToLower();
						GrowthType gt;

						switch (growtype)
						{
							case "bal" or "balanced":
								gt = GrowthType.Balanced;
								break;

							case "magic":
								gt = GrowthType.Magic;
								break;

							case "physical":
								gt = GrowthType.Physical;
								break;

							case "hybrid":
								gt = GrowthType.Hybrid;
								break;

							default:
								Console.WriteLine("Invalid input! Defaulting to Balanced.");
								gt = GrowthType.Balanced;
								break;
						}

						Console.WriteLine("Are you sure you want to use these stats? [Y/N]");
						Console.WriteLine($"Health: {100 + ints[0]}, Mana: {50 + ints[1]}, Strength: {10 + ints[2]}, Magic Strength: {10 + ints[3]}, Defense: {10 + ints[4]}, Dexterity: {10 + ints[5]}");
						Console.WriteLine($"Growth Type: {gt}");
						string confirmation = Console.ReadLine()?.Trim().ToUpper();
						if (confirmation == "Y")
						{
							EntityStats stats = new EntityStats(
								_health: 100 + ints[0],
								_mana: 50 + ints[1],
								_str: 10 + ints[2],
								_mstr: 10 + ints[3],
								_def: 10 + ints[4],
								_dex: 10 + ints[5]
								);

							Role r = new Role(roleName, roleDesc, gt);
							Player p = new Player(playerName, r, stats, ints.ToArray());
							return p;
						}
						else
						{
							Console.WriteLine("Re-enter your stats.");
							continue;
						}
					}

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				log.Log(LogLevel.ERROR, ex.Message);
				return null;
			}
			return null;
		}
		public void PrintInventory()
		{
			if (Inventory != null || Inventory.Count <= 0)
			{
				foreach (var item in Inventory)
				{
					if (item is Weapon wx)
					{
						Console.WriteLine("[ITEM]");
						Console.WriteLine($"Name: {wx.Name}");
						Console.WriteLine($"Description: {wx.Description}");
						Console.WriteLine($"Type: {wx.WeaponType}");
						Console.WriteLine($"Rarity:{wx.Rarity}");
						Console.WriteLine();
						Console.WriteLine($"Weight: {wx.Weight}");
						Console.WriteLine($"Value: {wx.Value}");
						Console.WriteLine($"Specialization: {wx.Specialization}");
						Console.WriteLine();
						Console.WriteLine($"Damage: {wx.WeaponDamage}");
						Console.WriteLine("[EFFECTS]");
						foreach (Effect eff in wx.Effects)
						{
							// TODO: logic
						}
						Console.WriteLine("[ENCHANTMENTS]");
						Console.WriteLine($"	Enchantment Slots: {wx.EnchantmentSlots}");
						foreach (Enchantment enc in wx.Enchantments)
						{
							//TODO logic
						}
					}
					else
					{

					}
					Console.WriteLine("");
				}
			}
			else
			{
				Console.WriteLine("	- Empty");
			}
			Console.WriteLine("---------------------------");
		}
		public void RedistributeStats()
		{
			try
			{
				bool Running = true;
				do
				{
					Console.WriteLine("Redistribute your stats.");
					Console.WriteLine("HEALTH, MANA, STRENGTH, MAGIC STRENGTH, DEFENSE, DEXTERITY.");
					Console.WriteLine($"You have {AllocationPoints} points to allocate.");
					string input = Utility.ValidateInput("Format your stats like this. (0 0 0 0 0 0)");

					string[] inputArr = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					List<int> intList = new List<int>();

					foreach (string i in inputArr)
					{
						if (int.TryParse(i, out int result))
						{
							intList.Add(result);
						}
						else
						{

						}
					}

					int sum = intList.Sum();

					if (intList.Count < 6)
					{
						Console.WriteLine("You must enter a number for each stat!");
					}

					if (sum > AllocationPoints)
					{
						Console.WriteLine($"The sum of these points exceeds {AllocationPoints} points.");
					}
					else if (sum < AllocationPoints)
					{
						Console.WriteLine($"You have {AllocationPoints - sum} missing points to allocate.");
					}
					else
					{
						// calculate base stats
						// apply changed stats to object
						// return 

						// base stats
						int baseHealth = this.Stats.Health - StatDistribution[0];
						int baseMana = this.Stats.Mana - StatDistribution[1];
						int baseStr = this.Stats.Strength - StatDistribution[2];
						int baseMStr = this.Stats.MagicStr - StatDistribution[3];
						int baseDef = this.Stats.Defense - StatDistribution[4];
						int baseDex = this.Stats.Dexterity - StatDistribution[5];

						// apply updated stats to object
						this.Stats.Health = baseHealth + intList[0];
						this.Stats.Mana = baseMana + intList[1];
						this.Stats.Strength = baseStr + intList[2];
						this.Stats.MagicStr = baseMStr + intList[3];
						this.Stats.Defense = baseDef + intList[4];
						this.Stats.Dexterity = baseDex + intList[5];

						// change stat distribution
						StatDistribution = intList.ToArray();
						Console.WriteLine("-------------------------------");
						Console.WriteLine($"Health: {this.Stats.Health}");
						Console.WriteLine($"Mana: {this.Stats.Mana}");
						Console.WriteLine($"Strength: {this.Stats.Strength}");
						Console.WriteLine($"Magic Strength: {this.Stats.MagicStr}");
						Console.WriteLine($"Defense: {this.Stats.Defense}");
						Console.WriteLine($"Dexterity: {this.Stats.Dexterity}");
						string confirm = Utility.ValidateInput("Are you happy with these stats? [Y/N]");

						if (confirm == "Y")
						{
							Running = false;
						}
						else
						{
							continue;
						}
					}
				}
				while (Running == true);
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}

	}

}

