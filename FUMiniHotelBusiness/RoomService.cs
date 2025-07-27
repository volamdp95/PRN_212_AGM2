using FUMiniHotelDataAccess.Models;
using FUMiniHotelRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUMiniHotelBusiness
{
    public class RoomService
    {
        private readonly IGenericRepository<RoomInformation> _roomRepo;
        private readonly IGenericRepository<RoomType> _roomTypeRepo;
        private readonly FUMiniHotelManagementContext _context;

        public RoomService()
        {
            var options = new DbContextOptionsBuilder<FUMiniHotelManagementContext>()
                .UseSqlServer("Server=localhost,1433;Database=FUMiniHotelManagement;User ID=sa;Password=123456;TrustServerCertificate=True")
                .Options;

            _context = new FUMiniHotelManagementContext(options);
            _roomRepo = new GenericRepository<RoomInformation>(_context);
            _roomTypeRepo = new GenericRepository<RoomType>(_context);
        }

        public async Task<List<RoomInformation>> GetAllAsync()
        {
            var all = await _roomRepo.GetAllAsync();
            foreach (var room in all)
            {
                room.RoomType = _context.RoomTypes.FirstOrDefault(t => t.RoomTypeID == room.RoomTypeID);
            }
            return all;
        }

        public async Task<List<RoomType>> GetRoomTypesAsync()
        {
            return await _roomTypeRepo.GetAllAsync();
        }

        public async Task AddAsync(RoomInformation room)
        {
            await _roomRepo.AddAsync(room);
            await _roomRepo.SaveAsync();
        }

        public async Task UpdateAsync(RoomInformation room)
        {
            _roomRepo.Update(room);
            await _roomRepo.SaveAsync();
        }

        public async Task DeleteAsync(RoomInformation room)
        {
            _roomRepo.Delete(room);
            await _roomRepo.SaveAsync();
        }

        public async Task<List<RoomInformation>> SearchRoomsAsync(string search)
        {
            var all = await _roomRepo.GetAllAsync();

            foreach (var room in all)
            {
                room.RoomType = _context.RoomTypes.FirstOrDefault(t => t.RoomTypeID == room.RoomTypeID);
            }

            if (string.IsNullOrWhiteSpace(search)) return all;

            string keyword = search.ToLower();

            return all.Where(r =>
                (!string.IsNullOrEmpty(r.RoomNumber) && r.RoomNumber.ToLower().Contains(keyword)) ||
                (!string.IsNullOrEmpty(r.RoomType?.RoomTypeName) && r.RoomType.RoomTypeName.ToLower().Contains(keyword))
            ).ToList();
        }
    }
}
