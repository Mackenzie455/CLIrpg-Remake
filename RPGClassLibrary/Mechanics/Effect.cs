namespace RPGClassLibrary.Mechanics
{
	public enum EffectType
	{

	}
	public class Effect
	{
		public string Name { get; set; }
		public EffectType Type { get; set; }
		public int Strength { get; set; }
		public int Duration { get; set; }
		public int Chance { get; set; }

		public Effect(string name, EffectType type, int strength, int duration, int chance)
		{
			Name = name;
			Type = type;
			Strength = strength;
			Duration = duration;
			Chance = Math.Clamp(chance, 0, 100);
		}

		public void ExecuteEffect()
		{

		}
	}
}
