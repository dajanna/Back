using FlightsReservation.Data;
using FlightsReservation.Interface;
using FlightsReservation.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace FlightsReservation.Repository
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;


        private readonly string _connectionString;

        public StatusRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection"); 
        }

       
      

        public ICollection<Status> GetListOfStatus()
        {
            return _context.Statuses.ToList();
        }

        public Status GetStatus(int id)
        {
            return _context.Statuses.Where(a => a.Id == id).FirstOrDefault();
        }



        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }



        public void InsertStatus(string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("InUpStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;



                    command.Parameters.AddWithValue("@Name", name);


                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStatus(int statusId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("InUpStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                   
                    command.Parameters.AddWithValue("@StatusID", statusId);
                    command.Parameters.AddWithValue("@Name", name);


                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool StatusExists(int id)
        {
            return _context.Statuses.Any(c => c.Id == id);
        }

    }
}
