namespace TripsServiceDAL.Interfaces;

public interface ISoftDelete
{
	public bool IsDeleted { get; set; }
}