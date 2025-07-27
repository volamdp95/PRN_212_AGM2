
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUMiniHotelDataAccess.Models;
using FUMiniHotelRepositories;

namespace FUMiniHotelBusiness
{
    public class CustomerService
    {
        private readonly FUMiniHotelManagementContext _context;
        private readonly IGenericRepository<Customer> _repo;

        public CustomerService()
        {
            var options = new DbContextOptionsBuilder<FUMiniHotelManagementContext>()
                .UseSqlServer("Server=localhost,1433;Database=FUMiniHotelManagement;User ID=sa;Password=123456;TrustServerCertificate=True")
                .Options;

            _context = new FUMiniHotelManagementContext(options);
            _repo = new GenericRepository<Customer>(_context);
        }

        public async Task<List<Customer>> GetCustomersAsync(string search = "")
        {
            var all = await _repo.GetAllAsync();
            if (string.IsNullOrWhiteSpace(search)) return all;

            string s = search.ToLower();
            return all.Where(c =>
                (!string.IsNullOrEmpty(c.CustomerFullName) && c.CustomerFullName.ToLower().Contains(s)) ||
                (!string.IsNullOrEmpty(c.EmailAddress) && c.EmailAddress.ToLower().Contains(s))
            ).ToList();
        }

        public async Task AddAsync(Customer c)
        {
            await _repo.AddAsync(c);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(Customer c)
        {
            _repo.Update(c);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(Customer c)
        {
            _repo.Delete(c);
            await _repo.SaveAsync();
        }
    }
}
