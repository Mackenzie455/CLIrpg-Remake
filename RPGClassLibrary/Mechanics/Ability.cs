using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGClassLibrary.Mechanics
{
	public enum AbilityTier
	{
		First,
		Second,
		Third,
		Fourth,
		Fifth
	}

	public class Ability
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public AbilityTier Tier { get; set; }
		public int ManaCost { get; set; }
		public int Cooldown { get; set; }
		public List<Effect> Effects { get; set; }

		public Ability(string name, string description, AbilityTier tier, int manaCost, int cooldown)
		{
			Name = name;
			Description = description;
			Tier = tier;
			ManaCost = manaCost;
			Cooldown = cooldown;
		}
	}
}
