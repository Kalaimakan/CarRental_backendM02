using Carrental.Entities;

namespace Carrental.IRepositries
{
    public interface IManagerRepository
    {
        Task<List<Rental>> GetAllRentals();
        Task<Rental> GetRentalByID(Guid rentalId);
        Task<Rental> AddRental(Rental rental);
        Task<Rental> RentalAccept(Rental rental);
    }
}
