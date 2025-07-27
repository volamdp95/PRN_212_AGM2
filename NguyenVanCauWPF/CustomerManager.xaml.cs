using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using NguyenVanCauWPF.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace NguyenVanCauWPF
{
    public partial class CustomerManager : Window
    {
        private readonly CustomerService _customerService;
        private List<Customer> _customers;

        public CustomerManager()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _ = LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync(string search = "")
        {
            _customers = await _customerService.GetCustomersAsync(search);
            dgCustomer.ItemsSource = _customers;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            await LoadCustomersAsync(txtSearch.Text.Trim());
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomerDialog();
            if (dialog.ShowDialog() == true)
            {
                await LoadCustomersAsync();
            }
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomer.SelectedItem is Customer selected)
            {
                var dialog = new CustomerDialog(selected);

                if (dialog.ShowDialog() == true)
                {
                    
                    await _customerService.UpdateAsync(selected); 
                    await LoadCustomersAsync();
                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomer.SelectedItem is Customer selected)
            {
                var result = MessageBox.Show("Xác nhận xoá khách hàng?", "Xác nhận",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _customerService.DeleteAsync(selected);
                    await LoadCustomersAsync();
                }
            }
        }
    }
}
