namespace Edile.Utils
{
	public class FamilyTypeID<T>
	{
		public static int Get<K>()
		{
			return TypeCounter<K>.ID;
		}

		public static int Get()
		{
			return FamilyTypeID<T>.count;
		}

		private static class TypeCounter<K>
		{
			public static readonly int ID;
			static TypeCounter()
			{
				TypeCounter<K>.ID = FamilyTypeID<T>.count++;
			}
		}

		private static int count = 0;
	}
}
