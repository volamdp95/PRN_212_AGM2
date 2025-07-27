using FUMiniHotelBusiness;
using FUMiniHotelDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace NguyenVanCauWPF.Views
{
    public partial class RoomDialog : Window
    {
        private readonly RoomService _service;
        private bool _isEdit;
        public RoomInformation Room { get; set; }
        public List<RoomType> RoomTypes { get; set; }

        public RoomDialog(RoomInformation? existing = null)
        {
            InitializeComponent();
            _service = new RoomService();

            if (existing == null)
            {
                Room = new RoomInformation
                {
                    RoomStatus = 1
                };
                _isEdit = false;
            }
            else
            {
                Room = existing;
                _isEdit = true;
            }

            this.DataContext = this;
            _ = LoadDataAsync();
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            RoomTypes = await _service.GetRoomTypesAsync();
            cboRoomType.ItemsSource = RoomTypes;
            cboRoomType.SelectedValue = Room.RoomTypeID;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboRoomType.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn loại phòng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ❗ Gán lại RoomTypeID, KHÔNG gán RoomType để tránh bị EF track trùng
                Room.RoomTypeID = (int)cboRoomType.SelectedValue;
                Room.RoomType = null;

                if (_isEdit)
                    await _service.UpdateAsync(Room);
                else
                    await _service.AddAsync(Room);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

