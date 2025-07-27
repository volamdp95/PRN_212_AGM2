using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using System;
using System.Linq;
using System.Windows;

namespace NguyenVanCauWPF
{
    public partial class BookingHistory : Window
    {
        private readonly string _email;
        private readonly CustomerService _customerService;
        private readonly BookingService _bookingService;

        public BookingHistory(string email)
        {
            InitializeComponent();
            _email = email;
            _customerService = new CustomerService();
            _bookingService = new BookingService();
            LoadBookingHistory();
        }

        private async void LoadBookingHistory()
        {
            var customer = (await _customerService.GetCustomersAsync(_email)).FirstOrDefault();
            if (customer == null)
            {
                MessageBox.Show("Customer not found.");
                this.Close();
                return;
            }

            var reservations = await _bookingService.GetBookingReservationsAsync();
            var details = await _bookingService.GetBookingDetailsAsync();

            var result = from r in reservations
                         join d in details on r.BookingReservationID equals d.BookingReservationID
                         where r.CustomerID == customer.CustomerID
                         select new
                         {
                             r.BookingReservationID,
                             d.RoomID,
                             d.StartDate,
                             d.EndDate,
                             d.ActualPrice,
                             r.TotalPrice
                         };

            dgBookings.ItemsSource = result.ToList();
        }

        private void BackToProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new Profile(_email);
            profileWindow.Show();
            this.Close();
        }
    }
}