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
        private BookingService _service;

        public BookingManager()
        {
            InitializeComponent();
            _service = new BookingService();
            LoadBookingList();
        }

        private void LoadBookingList()
        {
            var bookings = _service.GetAllBookings();
            dgBookingList.ItemsSource = bookings;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookingDialog();
            if (dialog.ShowDialog() == true)
            {
                LoadBookingList();
            }
        }

        //private void BtnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    var selected = dgBookingList.SelectedItem as BookingReservation;
        //    if (selected != null)
        //    {
        //        var dialog = new BookingDialog(selected.BookingReservationID);
        //        if (dialog.ShowDialog() == true)
        //        {
        //            LoadBookingList();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Chọn một booking để chỉnh sửa.");
        //    }
        //}

        //private void dgBookingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedBooking = dgBookingList.SelectedItem as BookingReservation;
        //    if (selectedBooking != null)
        //    {
        //        dgBookingDetails.ItemsSource = selectedBooking.BookingDetails.ToList();
        //    }
        //    else
        //    {
        //        dgBookingDetails.ItemsSource = null;
        //    }
        //}
    }
}
