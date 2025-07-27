using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NguyenVanCauWPF.Views
{
    public partial class BookingDialog : Window
    {
        private readonly BookingService _service;
        private readonly BookingReservation _editing;
        private List<BookingDetail> _details = new();

        public BookingDialog()
        {
            InitializeComponent();
            _service = new BookingService();
        }

        public BookingDialog(BookingReservation existing) : this()
        {
            _editing = existing;
            dpBookingDate.SelectedDate = existing.BookingDate?.ToDateTime(new TimeOnly(0, 0));
            txtCustomerID.Text = existing.CustomerID.ToString();
            txtStatus.Text = existing.BookingStatus?.ToString();
            txtTotalPrice.Text = existing.TotalPrice?.ToString();
            _details = existing.BookingDetails.ToList();
            dgDetails.ItemsSource = _details;
        }

        private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomDetailDialog();
            if (dialog.ShowDialog() == true)
            {
                _details.Add(dialog.Result);
                dgDetails.ItemsSource = null;
                dgDetails.ItemsSource = _details;
                txtTotalPrice.Text = _details.Sum(d => d.ActualPrice ?? 0).ToString();
            }
        }

        private void BtnRemoveRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgDetails.SelectedItem is BookingDetail selected)
            {
                _details.Remove(selected);
                dgDetails.ItemsSource = null;
                dgDetails.ItemsSource = _details;
                txtTotalPrice.Text = _details.Sum(d => d.ActualPrice ?? 0).ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!dpBookingDate.SelectedDate.HasValue ||
                !int.TryParse(txtCustomerID.Text, out int customerID) ||
                !byte.TryParse(txtStatus.Text, out byte status))
            {
                MessageBox.Show("Invalid input");
                return;
            }

            // ⚠️ Luôn tạo booking mới, chỉ giữ ID nếu là update
            var booking = new BookingReservation
            {
                BookingReservationID = _editing?.BookingReservationID ?? 0, // ID cũ nếu là edit
                BookingDate = DateOnly.FromDateTime(dpBookingDate.SelectedDate.Value),
                CustomerID = customerID,
                BookingStatus = status,
                TotalPrice = _details.Sum(d => d.ActualPrice ?? 0)
            };

            if (_editing == null)
                _service.AddBookingWithDetails(booking, _details);
            else
                _service.UpdateBookingWithDetails(booking, _details);

            this.DialogResult = true;
            this.Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}