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
    public partial class RoomManager : Window
    {
        private readonly RoomService _service;
        private List<RoomInformation> _rooms;

        public RoomManager()
        {
            InitializeComponent();
            _service = new RoomService();
            _ = LoadRoomsAsync();
        }

        private async Task LoadRoomsAsync(string search = "")
        {
            _rooms = await _service.SearchRoomsAsync(search);
            dgRoom.ItemsSource = _rooms;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomDialog();
            if (dialog.ShowDialog() == true)
                await LoadRoomsAsync();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dgRoom.SelectedItem is RoomInformation selected)
            {
                var dialog = new RoomDialog(selected);
                if (dialog.ShowDialog() == true)
                    await LoadRoomsAsync();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRoom.SelectedItem is RoomInformation selected)
            {
                var result = MessageBox.Show("Xóa phòng này?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _service.DeleteAsync(selected);
                    await LoadRoomsAsync();
                }
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string search = txtSearch.Text.Trim();
            await LoadRoomsAsync(search);
        }
    }
}
