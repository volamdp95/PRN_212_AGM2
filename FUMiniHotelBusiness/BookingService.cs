using FUMiniHotelDataAccess.Models;
using FUMiniHotelRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUMiniHotelBusiness
{
    public class BookingService
    {
        private readonly FUMiniHotelManagementContext _context;

        public BookingService()
        {
            _context = new FUMiniHotelManagementContext();
        }

        public List<BookingReservation> GetAllBookings()
        {
            return _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .ToList();
        }

        public BookingReservation? GetBookingById(int id)
        {
            return _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .FirstOrDefault(b => b.BookingReservationID == id);
        }

        public void AddBooking(BookingReservation booking)
        {
            _context.BookingReservations.Add(booking);
            _context.SaveChanges();
        }

        public void UpdateBooking(BookingReservation booking)
        {
            _context.BookingReservations.Update(booking);
            _context.SaveChanges();
        }

        public void DeleteBooking(int id)
        {
            var booking = _context.BookingReservations
                .Include(b => b.BookingDetails)
                .FirstOrDefault(b => b.BookingReservationID == id);

            if (booking != null)
            {
                _context.BookingDetails.RemoveRange(booking.BookingDetails);
                _context.BookingReservations.Remove(booking);
                _context.SaveChanges();
            }
        }

        public List<BookingReservation> GetBookingsByDateRange(DateOnly start, DateOnly end)
        {
            return _context.BookingReservations
                .Include(b => b.Customer)
                .Include(b => b.BookingDetails)
                    .ThenInclude(d => d.Room)
                .Where(b => b.BookingDetails.Any(d => d.StartDate >= start && d.EndDate <= end))
                .OrderByDescending(b => b.BookingDetails.Min(d => d.StartDate))
                .ToList();
        }

        // Lấy danh sách tất cả BookingReservation
        public async Task<List<BookingReservation>> GetBookingReservationsAsync()
        {
            return await _context.BookingReservations.ToListAsync();
        }

        // Lấy danh sách tất cả BookingDetail
        public async Task<List<BookingDetail>> GetBookingDetailsAsync()
        {
            return await _context.BookingDetails.ToListAsync();
        }

        public void AddBookingWithDetails(BookingReservation booking, List<BookingDetail> details)
        {
            _context.BookingReservations.Add(booking);
            _context.SaveChanges();

            foreach (var d in details)
            {
                d.BookingReservationID = booking.BookingReservationID;
                _context.BookingDetails.Add(d);
            }

            _context.SaveChanges();
        }

        public void UpdateBookingWithDetails(BookingReservation updatedBooking, List<BookingDetail> newDetails)
        {
            var existing = _context.BookingReservations
                .Include(b => b.BookingDetails)
                .FirstOrDefault(b => b.BookingReservationID == updatedBooking.BookingReservationID);

            if (existing != null)
            {
                // Cập nhật các thuộc tính cơ bản
                _context.Entry(existing).CurrentValues.SetValues(updatedBooking);

                // Xóa toàn bộ BookingDetails cũ
                _context.BookingDetails.RemoveRange(existing.BookingDetails);

                // Gán BookingReservationID và thêm lại BookingDetails mới
                foreach (var detail in newDetails)
                {
                    detail.BookingReservationID = existing.BookingReservationID;
                    _context.BookingDetails.Add(detail);
                }

                _context.SaveChanges();
            }
        }
    }
}
