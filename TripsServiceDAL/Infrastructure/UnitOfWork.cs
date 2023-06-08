using TripsServiceDAL.Repos;

namespace TripsServiceDAL.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        readonly TripsDBContext _context;

        private CommentRepository _commentRepository;
        private ImageRepository _imageRepository;
        private RoutePointRepository _routePointRepository;
        private TripRepository _tripRepository;
        private UserRepository _userRepository;

        public UnitOfWork(TripsDBContext context)
        {
            _context = context;
        }

        public CommentRepository Comments
        {
            get
            {
                _commentRepository ??= new(_context);
                return _commentRepository;
            }
        }

        public ImageRepository Images
        {
            get
            {
                _imageRepository ??= new(_context);
                return _imageRepository;
            }
        }

        public RoutePointRepository RoutePoints
        {
            get
            {
                _routePointRepository ??= new(_context);
                return _routePointRepository;
            }
        }

        public TripRepository Trips
        {
            get
            {
                _tripRepository ??= new(_context);
                return _tripRepository;
            }
        }

        public UserRepository Users
        {
            get
            {
                _userRepository ??= new(_context);
                return _userRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
