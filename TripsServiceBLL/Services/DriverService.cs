using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class DriverService : IDriverService
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

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
		Driver driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
		DriverDetailsDTO dto = _mapper.Map<Driver, DriverDetailsDTO>(driver);
		return dto;
	}
}