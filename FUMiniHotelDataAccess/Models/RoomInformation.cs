using System;
using System.Collections.Generic;

namespace FUMiniHotelDataAccess.Models;

public partial class RoomInformation
{
    public int RoomID { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string? RoomDetailDescription { get; set; }

    public int? RoomMaxCapacity { get; set; }

    public int RoomTypeID { get; set; }

    public byte? RoomStatus { get; set; }

    public decimal? RoomPricePerDay { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual RoomType RoomType { get; set; } = null!;
}
