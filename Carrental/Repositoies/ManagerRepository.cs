using Carrental.Entities;
using Carrental.IRepositries;
using System.Data;
using System.Data.SqlClient;

namespace Carrental.Repositoies
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly string _connectionString;

        public ManagerRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<List<Rental>> GetAllRentals()
        {
            var rentals = new List<Rental>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Rentals", connection);

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    rentals.Add(new Rental
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        CCarId = reader.GetGuid(reader.GetOrdinal("CCarID")),
                        RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                        Status = reader.GetString(reader.GetOrdinal("Status"))

                    });
                }
            }
            return rentals;
        }

        public async Task<Rental> GetRentalByID(Guid rentalId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT * FROM Rentals WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rentalId);

                var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow);
                if (await reader.ReadAsync())
                {
                    return new Rental
                    {
                        id = reader.GetGuid(reader.GetOrdinal("Id")),
                        CustomerId = reader.GetGuid(reader.GetOrdinal("CustomerId")),
                        CCarId = reader.GetGuid(reader.GetOrdinal("CarID")),
                        RentalDate = reader.GetDateTime(reader.GetOrdinal("RentalDate")),
                        ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                        OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                    };
                }
                return null;
            }
        }



        public async Task<Rental> AddRental(Rental rental)
        {
            rental.id = Guid.NewGuid();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Rentals (Id, CustomerId, CarId,RentalDate,ReturnDate,OverDue, Status) VALUES (@Id, @CustomerId, @CarId,@RentalDate,@ReturnDate,@OverDue, @Status); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Id", rental.id);
                command.Parameters.AddWithValue("@CustomerId", rental.CustomerId);
                command.Parameters.AddWithValue("@CarId", rental.CCarId);
                command.Parameters.AddWithValue("@RentalDate", DateTime.Now);
                command.Parameters.AddWithValue("@ReturnDate", DBNull.Value);
                command.Parameters.AddWithValue("@OverDue", rental.OverDue);
                command.Parameters.AddWithValue("@Status", rental.Status);

                await command.ExecuteScalarAsync();

                return rental;
            }
        }

        public async Task<Rental> RentalAccept(Rental rental)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Rentals SET Status = @Status WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", rental.id);
                command.Parameters.AddWithValue("@Status", "Rent");

                await command.ExecuteNonQueryAsync();
                var selectCommand = new SqlCommand(
                    "SELECT * FROM Rentals WHERE Id = @Id", connection);
                selectCommand.Parameters.AddWithValue("@Id", rental.id);

                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Map the updated rental data
                        rental.Status = reader["Status"].ToString();
                        rental.ReturnDate = reader.GetDateTime(reader.GetOrdinal("ReturnDate"));
                        rental.OverDue = reader.GetBoolean(reader.GetOrdinal("OverDue"));
                        // Map other necessary fields
                    }
                }

                return rental;
            }
        }

    }
}
