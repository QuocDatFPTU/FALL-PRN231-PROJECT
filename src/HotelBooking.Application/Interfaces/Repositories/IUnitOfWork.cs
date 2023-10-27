using HotelBooking.Domain.Common;

namespace HotelBooking.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task CommitAsync();
        Task RollbackAsync();
    }
}
