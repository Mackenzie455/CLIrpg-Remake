using RPGClassLibrary.Actors;

namespace RPGClassLibrary.Mechanics
{
	public enum EffectType
	{
		None,
		Poison, Burn, Frozen, Slowness, Weakness, ManaSickness, Curse, // Debuffs
		Strength, Haste, Resistance, Regeneration, ManaRegen, Lifesteal  // Buffs
	}
	public class Effect
	{
		public string Name { get; set; }
		public EffectType Type { get; set; }
		public int Strength { get; set; }
		public int Duration { get; set; }
		public int Chance { get; set; }

		public Effect(string name, int strength, int duration, int chance, EffectType type)
		{
			Name = name;
			Strength = strength;
			Duration = duration;
			Chance = Math.Clamp(chance, 0, 100);
			Type = type;
		}
		public bool IsDebuff()
		{
			return Type is EffectType.Poison or EffectType.Burn or EffectType.Frozen
				or EffectType.Slowness or EffectType.Weakness or EffectType.ManaSickness or EffectType.Curse;
		}
		public bool IsBuff()
		{
			return Type is EffectType.Strength or EffectType.Haste or EffectType.Resistance
				or EffectType.Regeneration or EffectType.ManaRegen or EffectType.Lifesteal;
		}

		public static Effect EffectFactory()
		{
			//points based system like with playerfactory?
			//TODO: logic
			return null;
		}
	}
}
