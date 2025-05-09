﻿using RPGClassLibrary.Combat;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Operations;
using System.Text.Json.Serialization;

namespace RPGClassLibrary.Items
{
	public enum WeaponSpec
	{
		None,
		Physical,
		Magic,
		Hybrid
	}
	public class Weapon : Item
	{
		public string Description { get; set; }
		public string WeaponType { get; set; }

		public int WeaponLevel { get; set; } = 1;
		public int Weight { get; set; } = 50;
		public int Value { get; set; } = 0;
		public int WeaponDamage { get; set; } = 0;
		public int EnchantmentSlots { get; set; } = 3;

		public DamageType DamageType { get; set; }
		public WeaponSpec Specialization { get; set; }

		public List<Effect> Effects { get; set; }
		public List<Enchantment> Enchantments { get; set; }

		[JsonConstructor]
		public Weapon(int iD, string name, string weaponType, string description, DamageType damageType, WeaponSpec spec, ItemRarity rarity) : base(iD, name, rarity, ItemType.Weapon)
		{
			Description = description;
			WeaponType = weaponType;

			DamageType = damageType;
			Specialization = spec;

			Effects = new List<Effect>();
			Enchantments = new List<Enchantment>();
		}
		public Weapon() { }
		public Weapon SetStats(int level, int weight, int value, int dmg, int slots)
		{
			WeaponLevel = level;
			Weight = weight;
			Value = value;
			WeaponDamage = dmg;
			EnchantmentSlots = slots;

			return this;
			//return this allows the SetStats method to be chained to a weapon declaration
			//TODO: add AddEffect and AddEnchantment methods that use this
		}

		public static Weapon WeaponFactory(ItemRarity rarity)
		{
			try
			{
				string errMsg = "Invalid input!";
				bool running = true;
				Weapon weapon = null;
				while (running)
				{
					Random ran = new Random();
					string Name = Utility.ValidateInput("What is the name of your weapon?", errMsg);
					string Type = Utility.ValidateInput("What type of weapon is it? A staff? A club? A sword?", errMsg);
					string Desc = Utility.ValidateInput("How would you describe your weapon?", errMsg);

					DamageType dt;
					WeaponSpec ws;

					string DTChoice = Utility.ValidateInput($"What type of damage does your weapon do?{Environment.NewLine} [NONE / MAGIC / PHYSICAL / ICE / FIRE / ELECTRIC / POISON]", errMsg);
					string WSChoice = Utility.ValidateInput($"What type of user is your weapon best used by?{Environment.NewLine} [NONE / PHYSICAL / MAGIC / HYBRID]", errMsg);

					switch (DTChoice.ToLower().Trim())
					{
						case "none": dt = DamageType.None; break;
						case "physical" or "phys": dt = DamageType.Physical; break;
						case "magic": dt = DamageType.Magic; break;
						case "ice": dt = DamageType.Ice; break;
						case "electric": dt = DamageType.Electric; break;
						case "posion": dt = DamageType.Poison; break;
						case "fire": dt = DamageType.Fire; break;
						default:
							Console.WriteLine("Invalid input! Defaulting to NONE.");
							dt = DamageType.None;
							break;
					}

					switch (WSChoice.ToLower().Trim())
					{
						case "none": ws = WeaponSpec.None; break;
						case "physical" or "phys": ws = WeaponSpec.Physical; break;
						case "magic": ws = WeaponSpec.Magic; break;
						case "hybrid": ws = WeaponSpec.Hybrid; break;
						default:
							Console.WriteLine("Invalid input! Defaulting to NONE.");
							ws = WeaponSpec.None;
							break;
					}

					int level = 1;
					int weight = ran.Next(0, 150);
					int value = weight * (int)rarity;
					int damage = (level + 1) * weight;
					int slots = 3;

					Console.WriteLine("Are you sure you want to create this weapon? [Y/N]");
					Console.WriteLine($"{Name}, {Type}, {Desc} / {rarity}");
					Console.WriteLine($"{dt} / {ws}");
					Console.WriteLine($"LVL: {level}, WGT: {weight}, VAL: {value}, DMG: {damage}");
					string confirmation = Console.ReadLine().ToUpper().Trim();
					if (confirmation == "Y")
					{
						weapon = new Weapon(
						iD: 0,
						name: Name,
						weaponType: Type,
						description: Desc,
						damageType: dt,
						spec: ws,
						rarity: rarity
						)
						.SetStats(level, weight, value, damage, slots);

						return weapon;
					}
					else
					{
						Console.WriteLine("Re-input your stats.");
					}
					// TODO: handle effects and enchantments
				}
				return weapon;
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
				return null;
			}
		}
		public static Weapon EnemyWeaponFactory(int enemyDifficulty)
		{
			try
			{
				string basePath = Utility.GetBasePath();
				Random ran = new Random();

				int ID = 0;
				string[] descriptors = File.ReadAllLines(Path.Combine(basePath, "Assets", "Descriptors.txt"));
				string[] wType = File.ReadAllLines(Path.Combine(basePath, "Assets", "WeaponTypes.txt"));

				string weaponType = wType[ran.Next(0, wType.Length)];
				string weaponName = $"{descriptors[ran.Next(0, descriptors.Length)]} {weaponType}";
				string desc = null;

				int dtIndex = ran.Next(0, Enum.GetNames(typeof(DamageType)).Length);
				DamageType dt = dtIndex switch
				{
					0 => DamageType.None,
					1 => DamageType.Magic,
					2 => DamageType.Physical,
					3 => DamageType.Fire,
					4 => DamageType.Ice,
					5 => DamageType.Electric,
					6 => DamageType.Poison,
					_ => DamageType.None
				};

				int wsIndex = ran.Next(0, Enum.GetNames(typeof(WeaponSpec)).Length);
				WeaponSpec ws = wsIndex switch
				{
					0 => WeaponSpec.None,
					1 => WeaponSpec.Physical,
					2 => WeaponSpec.Magic,
					3 => WeaponSpec.Hybrid,
					_ => WeaponSpec.None
				};

				int rarityIndex = ran.Next(0, Enum.GetNames(typeof(ItemRarity)).Length);
				ItemRarity rarity = rarityIndex switch
				{
					0 => ItemRarity.Common,
					1 => ItemRarity.Uncommon,
					2 => ItemRarity.Rare,
					3 => ItemRarity.Extraordinary,
					4 => ItemRarity.Legendary,
					_ => ItemRarity.Common
				};

				//stat calculation
				int weaponLevel = 1 + ((enemyDifficulty / 2) + Settings.GlobalDifficultyModifier);
				int weaponWeight = ran.Next(5 + weaponLevel, 80 + (enemyDifficulty / 2));
				int weaponDamage = weaponWeight + ((enemyDifficulty / 3) + Settings.GlobalDifficultyModifier);
				int weaponValue = weaponWeight * ((int)rarity + 1);
				int weaponSlots = 3;


				//weapon declaration
				Weapon w = new Weapon(
					iD: ID,
					name: weaponName,
					weaponType: weaponType,
					description: desc,
					damageType: dt,
					spec: ws,
					rarity: rarity
					)
					.SetStats(
					level: weaponLevel, 
					weight: weaponWeight, 
					value: weaponValue, 
					dmg: weaponDamage, 
					slots: weaponSlots
					);

				return w;
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
			}

			return null;
		}
	}
}
