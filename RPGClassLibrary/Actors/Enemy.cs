using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGClassLibrary.Actors;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Items;
using RPGClassLibrary.Operations;

namespace RPGClassLibrary.Actors
{
	public enum EnemyClass
	{
		Basic = (int)1.5,
		Strong = (int)2.5,
		Boss = (int)3.5
	}
	public class Enemy : Entity
	{
		public int EXPValue { get; set; }
		public EnemyClass Tier { get; set; }
		public Enemy(string name, Role role, EntityStats stats, EnemyClass tier, int lvl, int expVal) : base(name, role)
		{
			Stats = stats;
			Money = 100;
			Level = lvl;
			EXPValue = expVal;
			Tier = tier;
			AllocationPoints = 20;
			Inventory = new List<Item>();
		}

		public static Enemy EnemyFactory(EnemyClass enemyTier, int playerLevel)
		{
			try
			{
				Random ran = new Random();
				string assetpath = Path.Combine(Utility.GetBasePath(), "Assets");
				//generate enemy name
				//generate enemy role
				//generate enemy stats and statDist
				//generate enemy weapon
				//scales with enemyDifficulty, GlobalDifficulty and the players level

				//enemy difficulty
				int baseDifficulty = (playerLevel / 2) + ran.Next(-2, 3);
				int enemyDifficulty = enemyTier switch
				{
					EnemyClass.Basic => 1 + Math.Max(1, baseDifficulty * - 2),
					EnemyClass.Strong => baseDifficulty * (int)EnemyClass.Strong,
					EnemyClass.Boss => baseDifficulty * (int)EnemyClass.Boss,
					_ => 10
				};

				//enemy name
				string[] descriptors = File.ReadAllLines(Path.Combine(assetpath, "Descriptors.txt"));
				string[] names = File.ReadAllLines(Path.Combine(assetpath, "EnemyNames.txt"));

				string enemyName = $"{descriptors[ran.Next(0, descriptors.Length)]} {names[ran.Next(0, names.Length)]}";

				//enemy role
				string[] roles = File.ReadAllLines(Path.Combine(assetpath, "RoleNames.txt"));
				string ERName = roles[ran.Next(0, roles.Length)];
				Role enemyRole = new Role(ERName, null, GrowthType.Balanced);

				//enemy stats
				int enemyHealth = 100 + (enemyDifficulty * 10) + (int)(100 * (Startup.GlobalDifficultyModifier * 0.1));
				int enemyMStr = 10 + (enemyDifficulty * 4);
				int enemyMana = 50 + (enemyDifficulty * 5) + (enemyMStr * 2);

				int enemyStr = 10 + (enemyDifficulty * 3) + (Startup.GlobalDifficultyModifier * 2);
				int enemyDef = 10 + (enemyDifficulty * 2);
				int enemyDex = 10 + (enemyDifficulty * 3);

				int enemyLevel = 1 + (int)Math.Pow(playerLevel + enemyDifficulty, 0.7);
				int enemyEXPVal = (enemyLevel * 5) + ((enemyDifficulty * 2) * Startup.GlobalDifficultyModifier);


				EntityStats enemyStats = new EntityStats(
					_health: enemyHealth,
					_mana: enemyMana,
					_str: enemyStr,
					_mstr: enemyMStr,
					_def: enemyDef,
					_dex: enemyDex
					);


				//enemy declaration
				Enemy en = new Enemy(
					name: enemyName,
					role: enemyRole,
					stats: enemyStats,
					lvl: enemyLevel,
					expVal: enemyEXPVal,
					tier: enemyTier
					);

				//enemy weapon
				//give several consumables maybe?
				Weapon temp = Weapon.EnemyWeaponFactory(enemyDifficulty);
				en.AddItemToInventory(temp);
				return en;
			}
			catch (Exception ex)
			{
				Utility.bLog.ExceptionLog(LogLevel.ERROR, ex);
				return null;
			}
		}
	}
}
