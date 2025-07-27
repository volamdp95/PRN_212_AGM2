using FUMiniHotelBusiness;
using System.Windows;

namespace NguyenVanCauWPF
{
    public partial class LoginWindow : Window
    {
        private readonly CustomerService _customerService;

        public LoginWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter Email and Password!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Admin login
            if (email == "admin@FUMiniHotelSystem.com" && password == "@@abc123@@")
            {
                MessageBox.Show("Login as Admin", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                var main = new MainWindow();
                main.Show();
                this.Close();
                return;
            }

            // Customer login
            var customers = await _customerService.GetCustomersAsync(email);
            var matched = customers.FirstOrDefault(c => c.EmailAddress == email && c.Password == password);

            if (matched != null)
            {
                MessageBox.Show("Login as Customer", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: Navigate to Customer Window if needed
            }
            else
            {
                MessageBox.Show("Invalid email or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
