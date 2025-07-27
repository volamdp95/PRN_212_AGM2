using FUMiniHotelDataAccess.Models;
using System;
using System.Windows;

namespace NguyenVanCauWPF.Views
{
    public partial class RoomDetailDialog : Window
    {
        public BookingDetail Result { get; private set; }

        public RoomDetailDialog()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRoomID.Text, out int roomID) ||
                !dpStartDate.SelectedDate.HasValue ||
                !dpEndDate.SelectedDate.HasValue ||
                !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid input!");
                return;
            }

            Result = new BookingDetail
            {
                RoomID = roomID,
                StartDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value),
                EndDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value),
                ActualPrice = price
            };

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