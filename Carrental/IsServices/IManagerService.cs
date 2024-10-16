using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;

namespace Carrental.IsServices
{
    public interface IManagerService
    {
        Task<List<RentalResponseDTO>> GetAllRentals();
        Task<RentalResponseDTO> GetRentalById(Guid id);
        Task<RentalResponseDTO> AddRental(RentalRequestDTO rentalRequestDTO);
        Task<RentalResponseDTO> RentalAccept(Guid id);
    }
}
