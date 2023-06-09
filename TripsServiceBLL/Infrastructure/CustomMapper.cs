using AutoMapper;

namespace TripsServiceBLL.Infrastructure
{
	public static class CustomMapper<TSource, TDest>
	{
		public static TDest Map(TSource entityToMap)
		{
			MapperConfiguration config = new(cfg => cfg.AddProfile<MappingProfile>());
			Mapper mapper = new(config);
			return mapper.Map<TDest>(entityToMap);
		}
	}
}
