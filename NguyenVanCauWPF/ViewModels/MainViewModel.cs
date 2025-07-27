using FUMiniHotelDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NguyenVanCauWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); LoadCustomers(); }
        }

        public ObservableCollection<Customer> Customers { get; set; } = new();
        public Customer SelectedCustomer { get; set; }

        public MainViewModel()
        {
            LoadCustomers();
        }

        public void LoadCustomers()
        {
            var options = new DbContextOptionsBuilder<FUMiniHotelManagementContext>()
                .UseSqlServer("Server=localhost,1433;Database=FUMiniHotelManagement;User ID=sa;Password=123456;TrustServerCertificate=True")
                .Options;

            using var context = new FUMiniHotelManagementContext(options);
            var query = context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string s = SearchText.ToLower();
                query = query.Where(c => c.CustomerFullName.ToLower().Contains(s) ||
                                         c.EmailAddress.ToLower().Contains(s));
            }

            Customers.Clear();
            foreach (var customer in query)
                Customers.Add(customer);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
