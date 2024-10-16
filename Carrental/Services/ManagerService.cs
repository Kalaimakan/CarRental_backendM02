using Carrental.Dtos.RequestDTO;
using Carrental.Dtos.ResponceDTO;
using Carrental.Entities;
using Carrental.IRepositries;
using Carrental.IsServices;
using Carrental.Repositoies;

namespace Carrental.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _Repository;

        public ManagerService(IManagerRepository repository)
        {

            _Repository = repository;
        }

        public async Task<List<RentalResponseDTO>> GetAllRentals()
        {
            var customer = await _Repository.GetAllRentals();

            var data = new List<RentalResponseDTO>();
            foreach (var item in customer)
            {
                var rentalresponse = new RentalResponseDTO
                {
                    id = item.id,
                    CCarId = item.CCarId,
                    CustomerId = item.CustomerId,
                    RentalDate = item.RentalDate,
                    ReturnDate = item.ReturnDate,
                    OverDue = item.OverDue,
                    Status = item.Status,
                };

                data.Add(rentalresponse);
            }

            return data;
        }

        public async Task<RentalResponseDTO> GetRentalById(Guid id)
        {
            var data = await _Repository.GetRentalByID(id);
            var rentalresp = new RentalResponseDTO
            {
                id = data.id,
                CustomerId = data.CustomerId,
                CCarId = data.CCarId,
                ReturnDate = data.ReturnDate,
                Status = data.Status,
                OverDue = data.OverDue,
                RentalDate = data.RentalDate,
            };

            return rentalresp;
        }

        public async Task<RentalResponseDTO> AddRental(RentalRequestDTO rentalRequestDTO)
        {
            var rental = new Rental
            {
                CustomerId = rentalRequestDTO.CustomerId,
                CCarId = rentalRequestDTO.CCarId,
                RentalDate = rentalRequestDTO.RentalDate,
                ReturnDate = rentalRequestDTO.ReturnDate,
            };

            var data = await _Repository.AddRental(rental);

            var rentalresp = new RentalResponseDTO
            {
                id = data.id,
                CustomerId = data.CustomerId,
                CCarId = data.CCarId,
                ReturnDate = data.ReturnDate,
                Status = data.Status,
                OverDue = data.OverDue,
                RentalDate = data.RentalDate,
            };

            return rentalresp;
        }

        public async Task<RentalResponseDTO> RentalAccept(Guid id)
        {
            var Rentdata = await _Repository.GetRentalByID(id);
            if (Rentdata.Status == "Pending")
            {
                var data = await _Repository.RentalAccept(Rentdata);

                var RentalRespon = new RentalResponseDTO
                {
                    id = data.id,
                    CustomerId = data.CustomerId,
                    CCarId = data.CCarId,
                    ReturnDate = data.ReturnDate,
                    Status = data.Status,
                    OverDue = data.OverDue,
                    RentalDate = data.RentalDate,
                };

                return RentalRespon;
            }
            else
            {
                return null;
            }
        }

    }
}
