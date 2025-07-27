using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
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

namespace NguyenVanCauWPF.Views
{
    /// <summary>
    /// Interaction logic for BookingDialog.xaml
    /// </summary>
    public partial class BookingDialog : Window
    {
        private readonly BookingService _service;
        private bool _isEdit;

        public BookingReservation Booking { get; set; }
        public DateTime? BookingDate
        {
            get => Booking.BookingDate?.ToDateTime(TimeOnly.MinValue);
            set => Booking.BookingDate = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        public List<Customer> Customers { get; set; }

        public BookingDialog(BookingReservation? existing = null)
        {
            InitializeComponent();
            _service = new BookingService();

            if (existing == null)
            {
                Booking = new BookingReservation
                {
                    BookingDate = DateOnly.FromDateTime(DateTime.Today),
                    BookingStatus = 1
                };
                _isEdit = false;
            }
            else
            {
                Booking = existing;
                _isEdit = true;
            }

            this.DataContext = this;
            //_ = LoadCustomersAsync();
        }

        //private async System.Threading.Tasks.Task LoadCustomersAsync()
        //{
        //    Customers = await _service.GetAllCustomersAsync();
        //    cboCustomer.ItemsSource = Customers;
        //    cboCustomer.SelectedValue = Booking.CustomerID;
        //}

        //private async void Save_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        Booking.CustomerID = (int)cboCustomer.SelectedValue;

        //        if (_isEdit)
        //            await _service.UpdateAsync(Booking);
        //        else
        //            await _service.AddAsync(Booking);

        //        this.DialogResult = true;
        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //    this.Close();
        //}
    }
}
