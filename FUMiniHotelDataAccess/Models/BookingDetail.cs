using System;
using System.Collections.Generic;

namespace FUMiniHotelDataAccess.Models;

public partial class BookingDetail
{
    public int BookingReservationID { get; set; }

    public int RoomID { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal? ActualPrice { get; set; }

    public virtual BookingReservation BookingReservation { get; set; } = null!;

    public virtual RoomInformation Room { get; set; } = null!;
}
