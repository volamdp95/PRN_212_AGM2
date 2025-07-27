using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using NguyenVanCauWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NguyenVanCauWPF
{
    public partial class BookingManager : Window
    {
        private readonly BookingService _service;

        public BookingManager()
        {
            InitializeComponent();
            _service = new BookingService();
            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = _service.GetAllBookings(); // đã Include BookingDetails + Customer
            dgBookingList.ItemsSource = bookings;
            dgBookingDetails.ItemsSource = null;
        }

        private void dgBookingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgBookingList.SelectedItem as BookingReservation;
            if (selected != null)
            {
                dgBookingDetails.ItemsSource = selected.BookingDetails.ToList();
            }
            else
            {
                dgBookingDetails.ItemsSource = null;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Views.BookingDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBookingList.SelectedItem as BookingReservation;
            if (selected == null)
            {
                MessageBox.Show("Please select a booking to edit.");
                return;
            }
            var fullBooking = _service.GetBookingById(selected.BookingReservationID);
            var dialog = new Views.BookingDialog(selected); // dùng constructor edit
            if (dialog.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBookingList.SelectedItem as BookingReservation;
            if (selected == null)
            {
                MessageBox.Show("Please select a booking to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure to delete this booking?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _service.DeleteBooking(selected.BookingReservationID);
                LoadBookings();
            }
        }
    }
}
