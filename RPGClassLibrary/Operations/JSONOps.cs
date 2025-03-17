using RPGClassLibrary.Actors;
using RPGClassLibrary.Combat;
using RPGClassLibrary.Items;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGClassLibrary.Operations
{
	public class JSONOps
	{
		public static LoggingService log = new LoggingService(Sender.BACK);
		static JsonSerializerOptions options = new JsonSerializerOptions()
		{
			WriteIndented = true,
			Converters = { new ItemConverter() } // deserialization breaks with this????
		};

		public class ItemConverter : JsonConverter<Item>
		{
			public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				var jsonDoc = JsonDocument.ParseValue(ref reader);
				var jsonObject = jsonDoc.RootElement;

				Console.WriteLine(jsonObject.ToString());

				int id = jsonObject.GetProperty("ID").GetInt32();
				string name = jsonObject.GetProperty("Name").GetString();
				ItemRarity rarity = (ItemRarity)jsonObject.GetProperty("Rarity").GetInt32();
				ItemType type = (ItemType)jsonObject.GetProperty("Type").GetInt32();

				// Handle Weapon type specifically
				if (type == ItemType.Weapon)
				{
					string weaponType = jsonObject.GetProperty("WeaponType").GetString();
					string description = jsonObject.GetProperty("Description").GetString();
					DamageType damageType = (DamageType)jsonObject.GetProperty("DamageType").GetInt32();
					WeaponSpec spec = (WeaponSpec)jsonObject.GetProperty("Specialization").GetInt32();

					int level = jsonObject.GetProperty("WeaponLevel").GetInt32();
					int weight = jsonObject.GetProperty("Weight").GetInt32();
					int value = jsonObject.GetProperty("Value").GetInt32();
					int dmg = jsonObject.GetProperty("WeaponDamage").GetInt32();
					int slots = jsonObject.GetProperty("EnchantmentSlots").GetInt32();
					

					return new Weapon(id, name, weaponType, description, damageType, spec, rarity).SetStats(level, weight, value, dmg, slots);
				}

				/* Handle other ItemTypes (e.g., Armor, Consumable)
				// Add the necessary code for other item types if needed
				if (type == ItemType.Armor)
				{
					// Create and return an Armor object (example)
					return new Armor(id, name, rarity); // Assume you have an Armor class
				}
				*/

				throw new JsonException($"Unknown item type: {type}");
			}

			public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
			{
				writer.WriteStartObject();
				writer.WriteNumber("ID", value.ID);
				writer.WriteString("Name", value.Name);
				writer.WriteNumber("Rarity", (int)value.Rarity);
				writer.WriteNumber("Type", (int)value.Type);

				// Serialize Weapon specific properties
				if (value is Weapon weapon)
				{
					writer.WriteString("WeaponType", weapon.WeaponType);
					writer.WriteString("Description", weapon.Description);

					writer.WriteNumber("WeaponLevel", weapon.WeaponLevel);
					writer.WriteNumber("Weight", weapon.Weight);
					writer.WriteNumber("Value", weapon.Value);
					writer.WriteNumber("WeaponDamage", weapon.WeaponDamage);
					writer.WriteNumber("EnchantmentSlots", weapon.EnchantmentSlots);

					writer.WriteNumber("DamageType", (int)weapon.DamageType);
					writer.WriteNumber("Specialization", (int)weapon.Specialization);
				}

				writer.WriteEndObject();
			}
		}



		public struct Serialize
		{
			public static void SerializePlayer(Player player)
			{
				try
				{
					string path = Path.Combine(Utility.GetBasePath(), "Output", "Characters", $"{player.Name}.player.json");
					string json = JsonSerializer.Serialize(player, options);

					File.WriteAllText(path, json);
				}
				catch (Exception ex)
				{
					log.Log(LogLevel.ERROR, ex.Message);
				}
			}
		}
		public struct Deserialize
		{
			public static Player DeserializePlayer(string playerName)
			{
				try
				{
					string path = Path.Combine(Utility.GetBasePath(), "Output", "Characters", $"{playerName}.player.json");
					string json = File.ReadAllText(path);
					Player p = JsonSerializer.Deserialize<Player>(json, options);
					return p;
				}
				catch (Exception ex)
				{
					log.Log(LogLevel.ERROR, ex.Message);
					return null;
				}
			}
		}
	}
}
