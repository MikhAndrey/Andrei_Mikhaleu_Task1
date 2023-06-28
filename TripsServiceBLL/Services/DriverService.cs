using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure.Exceptions;
using TripsServiceDAL.Interfaces;
using TripsServiceDAL.Utils;

namespace TripsServiceBLL.Services
{
	public class DriverService : IDriverService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public DriverService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IEnumerable<ReadDriverDTO> GetDriversOverall()
		{
			IEnumerable<Driver> drivers = _unitOfWork.Drivers.GetAll();
			IEnumerable<ReadDriverDTO> mappedDrivers = drivers.Select(_mapper.Map<Driver, ReadDriverDTO>)
				.OrderByDescending(el => el.AverageRating);
			return mappedDrivers;
		}

		public async Task<DriverDetailsDTO> GetDriverDetailsAsync(int driverId)
		{
			Driver? driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
			if (driver == null)
			{
				throw new EntityNotFoundException(UtilConstants.GetEntityNotExistsMessage<Driver>()());
			}
			DriverDetailsDTO dto = _mapper.Map<Driver, DriverDetailsDTO>(driver);
			return dto;
		}
	}
}
