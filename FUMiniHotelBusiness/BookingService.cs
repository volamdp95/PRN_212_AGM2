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
    }
}
