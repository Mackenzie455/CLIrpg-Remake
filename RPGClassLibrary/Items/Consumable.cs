using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGClassLibrary.Items
{
	public class Consumable : Item
	{
		public Consumable(int iD, string name, ItemRarity rarity, ItemType type) : base(iD, name, rarity, type)
		{

		}
	}
}
