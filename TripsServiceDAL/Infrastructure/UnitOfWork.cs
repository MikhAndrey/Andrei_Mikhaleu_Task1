using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceDAL.Interfaces;
using TripsServiceDAL.Repos;

namespace TripsServiceDAL.Infrastructure;

public class UnitOfWork : IDisposable, IUnitOfWork
{
	private readonly TripsDBContext _context;

	private ICommentRepository _commentRepository;
	private IDriverRepository _driverRepository;
	private IFeedbackRepository _feedbackRepository;
	private IImageRepository _imageRepository;
	private IRoutePointRepository _routePointRepository;
	private ITripRepository _tripRepository;
	private IUserRepository _userRepository;
	private IRoleRepository _roleRepository;
	private IChatMessageRepository _chatMessageRepository;
	private IChatRepository _chatRepository;
	private IChatParticipationRepository _chatParticipationRepository;

	private bool disposed;

	public UnitOfWork(TripsDBContext context)
	{
		_context = context;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	public ICommentRepository Comments
	{
		get
		{
			_commentRepository ??= new CommentRepository(_context);
			return _commentRepository;
		}
	}

	public IImageRepository Images
	{
		get
		{
			_imageRepository ??= new ImageRepository(_context);
			return _imageRepository;
		}
	}

	public IRoutePointRepository RoutePoints
	{
		get
		{
			_routePointRepository ??= new RoutePointRepository(_context);
			return _routePointRepository;
		}
	}

	public ITripRepository Trips
	{
		get
		{
			_tripRepository ??= new TripRepository(_context);
			return _tripRepository;
		}
	}

	public IUserRepository Users
	{
		get
		{
			_userRepository ??= new UserRepository(_context);
			return _userRepository;
		}
	}
	
	public IRoleRepository Roles
	{
		get
		{
			_roleRepository ??= new RoleRepository(_context);
			return _roleRepository;
		}
	}

	public IDriverRepository Drivers
	{
		get
		{
			_driverRepository ??= new DriverRepository(_context);
			return _driverRepository;
		}
	}

	public IFeedbackRepository Feedbacks
	{
		get
		{
			_feedbackRepository ??= new FeedbackRepository(_context);
			return _feedbackRepository;
		}
	}

	public IChatMessageRepository ChatMessages
	{
		get
		{
			_chatMessageRepository ??= new ChatMessageRepository(_context);
			return _chatMessageRepository;
		}
	}
	
	public IChatRepository Chats
	{
		get
		{
			_chatRepository ??= new ChatRepository(_context);
			return _chatRepository;
		}
	}
	
	public IChatParticipationRepository ChatParticipations
	{
		get
		{
			_chatParticipationRepository ??= new ChatParticipationRepository(_context);
			return _chatParticipationRepository;
		}
	}

	public async Task SaveAsync()
	{
		await _context.SaveChangesAsync();
	}

	public IDbContextTransaction BeginTransaction()
	{
		return _context.Database.BeginTransaction();
	}

	public virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				_context.Dispose();
			}

			disposed = true;
		}
	}
}
