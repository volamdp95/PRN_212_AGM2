using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using NguyenVanCauWPF.Views;
using System;
using System.Linq;
using System.Windows;

namespace NguyenVanCauWPF
{
    public partial class Profile : Window
    {
        private readonly CustomerService _customerService;
        private readonly string _email;
        private Customer _currentCustomer;

        public Profile(string email)
        {
            InitializeComponent();
            _email = email;
            _customerService = new CustomerService();
            LoadProfile();
        }

        private async void LoadProfile()
        {
            var customer = (await _customerService.GetCustomersAsync(_email)).FirstOrDefault();
            if (customer == null)
            {
                MessageBox.Show("Customer not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

            _currentCustomer = customer;

            txtName.Text = customer.CustomerFullName;
            txtPhone.Text = customer.Telephone;
            if (customer.CustomerBirthday.HasValue)
            {
                dpBirthday.SelectedDate = customer.CustomerBirthday.Value.ToDateTime(new TimeOnly(0, 0));
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null) return;

            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text) || dpBirthday.SelectedDate == null)
            {
                MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentCustomer.CustomerFullName = txtName.Text.Trim();
            _currentCustomer.Telephone = txtPhone.Text.Trim();
            _currentCustomer.CustomerBirthday = DateOnly.FromDateTime(dpBirthday.SelectedDate.Value);

            await _customerService.UpdateAsync(_currentCustomer);

            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewBooking_Click(object sender, RoutedEventArgs e)
        {
            var bookingWindow = new BookingHistory(_email);
            bookingWindow.ShowDialog();
        }
    }
}