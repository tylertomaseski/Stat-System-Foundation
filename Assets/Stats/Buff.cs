namespace Stats
{
	[System.Serializable]
	public struct Buff
	{
		public enum StackableSetting
		{
			Stacks,
			OnePerSource,
			OneOnly
		}

		//((stat + AddPreMultiply) * Multiply) + AddPostMultiply
		public enum TargetVariable
		{
			AddPreMultiply,
			Multiplier,
			AddPostMultiply,
			Length
		}

		public enum ExpirationType
		{
			Timed,
			Length
		}

		public int BuffOriginID;
		public int BuffID;
		public StackableSetting StackSetting;
		public TargetVariable BuffType;
		public ExpirationType BuffExpirationMode;
		public Stat TargetStat;
		public float Value;
		public float Duration;
		public BuffTemplate Template;
	}
}