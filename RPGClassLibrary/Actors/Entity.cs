using RPGClassLibrary.Items;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;
using System.Text.Json.Serialization;

namespace RPGClassLibrary.Actors
{
	public enum StatTypes
	{
		Health,
		Mana,
		Strength,
		MagicStr,
		Defense,
		Dexterity
	}
	public abstract class Entity
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public int EXP { get; set; }
		public int EXPToNextLevel { get; set; }
		public int Money { get; set; }

		//------------Stats-----------
		public Role Role { get; set; }
		public EntityStats Stats { get; set; }
		public int AllocationPoints { get; set; }
		public int[] StatDistribution { get; set; }

		//------------Belongings-----------
		public Dictionary<string, Item> Inventory { get; set; }
		public List<Effect> CurrentEffects { get; set; }

		[JsonIgnore]
		public EffectHandler EffectHandler { get; set; }

		//--------Constructor------------
		protected Entity(string name, Role role, int level = 1)
		{
			Name = name;
			Role = role;
			Level = level;
			EXP = 0;
			Money = 0;
			EXPToNextLevel = 100;

			Stats = new EntityStats();
			Inventory = new Dictionary<string, Item>();
			CurrentEffects = new List<Effect>();
			EffectHandler = new EffectHandler(this);
		}
		protected Entity() { }

		public class EntityStats
		{
			public int Health { get; set; }
			public int Mana { get; set; }
			public int Strength { get; set; }
			public int MagicStr { get; set; }
			public int Defense { get; set; }
			public int Dexterity { get; set; }

			public EntityStats() { }
			public EntityStats(int _health = 100, int _mana = 50, int _str = 10, int _mstr = 10, int _def = 10, int _dex = 10)
			{
				Health = _health;
				Mana = _mana;
				Strength = _str;
				MagicStr = _mstr;
				Defense = _def;
				Dexterity = _dex;
			}

			public void ShowStats()
			{
				Console.WriteLine($"Health: {this.Health}");
				Console.WriteLine($"Mana: {this.Mana}");
				Console.WriteLine($"Strength: {this.Strength}");
				Console.WriteLine($"Magic Strength: {this.MagicStr}");
				Console.WriteLine($"Defense: {this.Defense}");
				Console.WriteLine($"Dexterity: {this.Dexterity}");
			}
		}

		/// <summary>
		/// Adds a designated amount of experience to the Entity it's called from. 
		/// Handles specialised growth as well.
		/// </summary>
		/// <param name="amount">The amount of EXP to add.</param>
		public void GainExp(int amount)
		{
			EXP += amount;

			while (EXP >= EXPToNextLevel) // Handle multiple level-ups
			{
				EXP -= EXPToNextLevel; // Deduct EXP required for the level-up
				Level++;
				EXPToNextLevel = (int)(EXPToNextLevel * 1.2); // Increase EXP requirement for the next level

				AllocationPoints++;
				int GainAmount = 1 + (Level / 5); // Gain amount will increase by 1 every 5 levels
				int mod = 2;

				switch (Role.GrowthType)
				{
					case GrowthType.Balanced:
						Stats.Health += GainAmount;
						Stats.Mana += GainAmount;
						Stats.Strength += GainAmount;
						Stats.MagicStr += GainAmount;
						Stats.Defense += GainAmount;
						Stats.Dexterity += GainAmount;
						break;

					case GrowthType.Magic:
						Stats.Health += GainAmount;
						Stats.Strength += GainAmount * (int)1.5;
						Stats.Mana += GainAmount * mod;
						Stats.MagicStr += GainAmount * mod;
						Stats.Dexterity += GainAmount * mod;
						break;

					case GrowthType.Physical:
						Stats.Health += GainAmount * mod;
						Stats.Strength += GainAmount * mod;
						Stats.Defense += GainAmount * mod;
						Stats.Dexterity += GainAmount;
						break;

					case GrowthType.Hybrid:
						Stats.Health += GainAmount;
						Stats.Mana += GainAmount;
						Stats.Strength += GainAmount * (int)1.5; // Gains less than pure physical but more than balanced
						Stats.MagicStr += GainAmount * (int)1.5; // Gains less than pure magic but more than balanced
						Stats.Defense += GainAmount;
						Stats.Dexterity += GainAmount;
						break;
				}

				Console.WriteLine($"Level Up! {Name} is now Level {Level}!");
			}
		}
		public void Info()
		{
			Console.WriteLine("---------------------------");
			Console.WriteLine($"Name: {Name}");
			Console.WriteLine($"Level: {Level} | EXP: {EXP}, To Next: {EXPToNextLevel}");
			Console.WriteLine($"Money: {Money}");
			Console.WriteLine("Role:");
			Console.WriteLine($"	Role Name: {Role.Name}");
			Console.WriteLine($"	Description: {Role.Description}");
			Console.WriteLine($"	Growth Type: {Role.GrowthType}");
			Console.WriteLine("Stats:");
			Console.WriteLine($"    Health: {Stats.Health}");
			Console.WriteLine($"    Mana: {Stats.Mana}");
			Console.WriteLine($"    Strength: {Stats.Strength}");
			Console.WriteLine($"    Magic Strength: {Stats.MagicStr}");
			Console.WriteLine($"    Defense: {Stats.Defense}");
			Console.WriteLine($"    Dexterity: {Stats.Dexterity}");
			if (this is not Enemy)
			{
				Console.WriteLine($"Distribution: {String.Join(", ", StatDistribution)}");
			}
			Console.WriteLine($"Points to allocate: {AllocationPoints}");
			Console.WriteLine("---------------------------");
			Console.WriteLine("[Inventory]");
			this.PrintInventory();
			Console.WriteLine("---------------------------");
			Console.WriteLine("[Current Effects]");
			foreach (Effect eff in this.CurrentEffects)
			{
				Console.WriteLine($"	Name: {eff.Name}");
				Console.WriteLine($"	Effect Type: {eff.Type.ToString()}");
				Console.WriteLine($"	Duration: {eff.Duration}");
				Console.WriteLine($"	Strength: {eff.Strength} | Modifier: {eff.Modifier}");
				Console.WriteLine($"	Damage Per Turn: {eff.DamagePerTurn}");
				Console.WriteLine($"	Applied: {eff.EffectApplied}");
				Console.WriteLine();
			}
			Console.WriteLine();
			Console.WriteLine("---------------------------");
		}
		public void AddItemToInventory(Item item)
		{
			try
			{
				Inventory.Add(item.Name, item);
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void PrintInventory()
		{
			if (Inventory != null || Inventory.Count <= 0)
			{
				foreach (var item in Inventory)
				{
					if (item.Value is Weapon wx)
					{
						Console.WriteLine("[Weapon]");
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
							Console.WriteLine($"	Name: {eff.Name}");
							Console.WriteLine($"	Effect Type: {eff.Type.ToString()}");
							Console.WriteLine($"	Duration: {eff.Duration}");
							Console.WriteLine($"	Strength: {eff.Strength} | Modifier: {eff.Modifier}");
							Console.WriteLine($"	Damage Per Turn: {eff.DamagePerTurn}");
							Console.WriteLine($"	Applied: {eff.EffectApplied}");
							Console.WriteLine();
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
	}
}