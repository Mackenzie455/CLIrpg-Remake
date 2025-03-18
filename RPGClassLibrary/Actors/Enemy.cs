using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGClassLibrary.Actors;
using RPGClassLibrary.Mechanics;
using RPGClassLibrary.Items;

namespace RPGClassLibrary.Actors
{
	public enum EnemyRoles
	{
		None,
		Berserker,
		Lich,
		DarkMage,
		Goblin
	}
	public class Enemy : Entity
	{
		public Enemy(string name, Role role, EntityStats stats, int[] statDist) : base(name, role)
		{
			Stats = stats;
			Money = 100;
			StatDistribution = statDist;
			AllocationPoints = 20;
			Inventory = new List<Item>();
		}

		public static void EnemyFactory(int enemyDifficulty, int playerLevel)
		{
			//generate enemy name
			//generate enemy role
			//generate enemy stats and statDist
			//scales with enemyDifficulty, GlobalDifficulty and the players level
		}
	}
}
