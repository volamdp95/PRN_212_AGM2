using System;
using System.Collections.Generic;

namespace FUMiniHotelDataAccess.Models;

public partial class BookingReservation
{
    public int BookingReservationID { get; set; }

    public DateOnly? BookingDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public int CustomerID { get; set; }

    public byte? BookingStatus { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Customer Customer { get; set; } = null!;
}
