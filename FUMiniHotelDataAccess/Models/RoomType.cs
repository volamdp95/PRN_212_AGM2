using System;
using System.Collections.Generic;

namespace FUMiniHotelDataAccess.Models;

public partial class RoomType
{
    public int RoomTypeID { get; set; }

    public string RoomTypeName { get; set; } = null!;

    public string? TypeDescription { get; set; }

    public string? TypeNote { get; set; }

    public virtual ICollection<RoomInformation> RoomInformations { get; set; } = new List<RoomInformation>();
}
