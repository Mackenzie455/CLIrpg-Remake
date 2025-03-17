using RPGClassLibrary.Actors;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace RPGClassLibrary.Items
{
	public enum ItemRarity
	{
		Common,
		Uncommon,
		Rare,
		Extraordinary,
		Legendary,
	}
	public enum ItemType
	{
		Armor,
		Weapon,
		Consumable,
		Material
	}

	public abstract class Item //DONE: find a way to make this abstract
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public ItemRarity Rarity { get; set; }
		public ItemType Type { get; set; }

		[JsonConstructorAttribute]
		protected Item(int iD, string name, ItemRarity rarity, ItemType type)
		{
			ID = iD;
			Name = name;
			Rarity = rarity;
			Type = type;
		}
		protected Item() { }
	}
}
