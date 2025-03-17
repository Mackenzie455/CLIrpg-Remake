using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGClassLibrary.Mechanics
{
	public class Role
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public GrowthType GrowthType { get; set; }

		//------------------------------------//
		List<Ability> Abilities { get; set; }

		public Role(string name, string description = null, GrowthType gType = GrowthType.Balanced)
        {
            Name = name;
			Description = description;
			GrowthType = gType;
			List<Ability> Abilities = new List<Ability>();
        }
		public Role() { }
    }
	public enum Roles
	{
		None,
		Knight,
		Berserker,
		Archer,
		Mage,
		Rogue
	}
	public enum GrowthType
	{
		Balanced,
		Magic,
		Physical,
		Hybrid
	}

}
