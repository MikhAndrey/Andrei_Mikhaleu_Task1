namespace TripsServiceDAL.Utils
{
	public static class UtilConstants
	{
		public static UtilDelegates.StringGeneratorGenericDelegate<T> GetEntityNotFoundMessage<T>()
		{
			return () => $"Corresponding {typeof(T).Name.ToLower()} was not found";
		}

		public static UtilDelegates.StringGeneratorGenericDelegate<T> GetEntityNotExistsMessage<T>()
		{
			return () => $"This {typeof(T).Name.ToLower()} doesn't exist anymore";
		}
	}
}
