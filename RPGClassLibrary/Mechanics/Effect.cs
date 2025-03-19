using RPGClassLibrary.Actors;

namespace RPGClassLibrary.Mechanics
{
	public enum EffectDebuff
	{
		None,
		Poison,
		Burn,
		Frozen,
		Slowness,
		Weakness,
		ManaSickness
	}

	public enum EffectBuff
	{
		None, 
		Strength,
		Haste,
		Resistance,
		Regeneration,
		ManaRegen
	}
	public class Effect
	{
		public string Name { get; set; }
		public EffectDebuff Debuff { get; set; }
		public EffectBuff Buff { get; set; }
		public int Strength { get; set; }
		public int Duration { get; set; }
		public int Chance { get; set; }

		public Effect(string name, int strength, int duration, int chance, EffectBuff buff, EffectDebuff debuff)
		{
			Name = name;
			Strength = strength;
			Duration = duration;
			Chance = Math.Clamp(chance, 0, 100);
			Debuff = debuff;
			Buff = buff;
		}

		public void EffectBodyPopulate()
		{
			
		}
	}
}
