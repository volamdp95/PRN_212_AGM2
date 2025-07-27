using System;
using System.Windows;
using FUMiniHotelDataAccess.Models;
using FUMiniHotelBusiness;

namespace NguyenVanCauWPF.Views
{
    public partial class CustomerDialog : Window
    {
        private readonly CustomerService _service;
        private bool _isEdit;

        public Customer Customer { get; set; }



        public DateTime? Birthday
        {
            get => Customer.CustomerBirthday.HasValue ? Customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : null;
            set => Customer.CustomerBirthday = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
        }

        // ✅ Constructor dùng chung: thêm nếu null, sửa nếu truyền existing
        public CustomerDialog(Customer? existing = null)
        {
            InitializeComponent();
            _service = new CustomerService();

            if (existing == null)
            {
                Customer = new Customer();
                _isEdit = false;
            }
            else
            {
                Customer = existing; // ❗Không tạo mới, giữ nguyên object gốc
                pwdBox.Password = existing.Password;
                _isEdit = true;
            }

            this.DataContext = this;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer.Password = pwdBox.Password;

                if (_isEdit)
                    await _service.UpdateAsync(Customer);
                else
                {
                    Customer.CustomerStatus = 1;
                    await _service.AddAsync(Customer);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
