using RPGClassLibrary.Items;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;

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
		public List<Item> Inventory { get; set; }
		public List<Effect> CurrentEffects { get; set; }

		//--------Constructor------------
		protected Entity(string name, Role role, int level = 1)
		{
			Name = name;
			Role = role;
			Level = level;
			EXP = 0;
			Money = 0;
			EXPToNextLevel = 100;  // Example value
			Stats = new EntityStats();
			Inventory = new List<Item>();
			CurrentEffects = new List<Effect>();
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
			Console.WriteLine("Inventory:");
			Console.WriteLine($"	- Empty"); //Have proper inventory shown
			Console.WriteLine("---------------------------");
		}
		public void AddItemToInventory(Item item)
		{
			try
			{
				Inventory.Add(item);
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void AddEffect(Effect e)
		{
			try
			{

				var existingEffect = this.CurrentEffects.FirstOrDefault(effect => effect.Name == e.Name);
				if (existingEffect != null)
				{
					existingEffect.Duration = Math.Max(existingEffect.Duration, e.Duration);
					existingEffect.Strength = Math.Max(existingEffect.Strength, e.Strength);
				}
				else
				{
					this.CurrentEffects.Add(e);
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void EffectExecutor(int effectModifier = 10)
		{
			try
			{
				List<Effect> Expired = new List<Effect>();

				foreach (Effect effect in this.CurrentEffects)
				{
					if (effect.IsBuff())
					{
						ApplyBuff(effect, effectModifier);
					}
					else if (effect.IsDebuff())
					{
						ApplyDebuff(effect, effectModifier);
					}
				}

				//TODO: expiry logic here
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}

		public void ApplyBuff(Effect effect, int effectModifier)
		{
			try
			{
				ConsoleColor color = Settings.DefaultConsoleColor;
				Random ran = new Random();
				switch (effect.Type)
				{
					case EffectType.Regeneration: 

						break;

					case EffectType.ManaRegen:
						
						break;

					case EffectType.Haste: 

						break;

					case EffectType.Strength:
						int strength = effect.Strength + effectModifier;
						this.Stats.Strength = strength;

						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine($"{this.Name} has become stronger! Defense increased by {strength}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Resistance:
						int resistance = effect.Strength + effectModifier;
						this.Stats.Defense += resistance;

						Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{this.Name} has become more resistant! Defense increased by {resistance}!");
						Console.ForegroundColor = color;
						break;
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}
		}
		public void ApplyDebuff(Effect effect, int effectModifier)
		{
			try
			{
				ConsoleColor color = Settings.DefaultConsoleColor;
				Random ran = new Random();
				switch (effect.Type)
				{
					case EffectType.Burn or EffectType.Poison:
						int damage = Math.Max(1, 1 + (effectModifier * effect.Strength) - this.Stats.Strength);
						this.Stats.Health -= damage;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{this.Name} took {damage} damage from the {effect.Type.ToString().ToLower()}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Frozen:
						int freezeChance = ran.Next(0, 20) + effect.Strength;
						if (freezeChance > 18)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"{Name} is frozen solid and cannot act this turn!");
							Console.ForegroundColor = color;
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine($"{Name}'s accuracy is reduced due to being frozen.");
							Console.ForegroundColor = color;

							//TODO: logic
						}
						break;

					case EffectType.Weakness:
						int weakness = effect.Strength + (effectModifier + 1);
						this.Stats.Strength -= weakness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{this.Name} has lost {weakness} strength due to their weakness!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Slowness:
						int slowness = effect.Strength + (effectModifier + 1);
						this.Stats.Dexterity -= slowness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{this.Name} has lost {slowness} dexterity due to their slowness! Reaction time and dodging is harder!");
						Console.ForegroundColor = color;
						break;

					case EffectType.ManaSickness:
						int sickness = effect.Strength + (effectModifier * 2);
						this.Stats.Mana -= sickness;
						this.Stats.MagicStr -= sickness;
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"{this.Name} has come down with mana sickness! Mana and Magic Strength reduced by {sickness}!");
						Console.ForegroundColor = color;
						break;

					case EffectType.Curse:
						int curse = (int)Math.Round((decimal)effect.Strength + effectModifier, 2);

						this.Stats.Health -= Math.Max(0, curse);
						this.Stats.Mana -= Math.Max(0, curse);
						this.Stats.Strength -= Math.Max(0, curse);
						this.Stats.MagicStr -= Math.Max(0, curse);
						this.Stats.Defense -= Math.Max(0, curse);
						this.Stats.Dexterity -= Math.Max(0, curse);

						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"A dark magic attack curses {this.Name}! All stats reduced by {curse}!");
						Console.ForegroundColor = color;
						break;
				}
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}

		}
	}
}