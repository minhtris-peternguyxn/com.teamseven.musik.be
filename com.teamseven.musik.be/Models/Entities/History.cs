using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class History
{
    public int HistoryId { get; set; }

    public int? TrackId { get; set; }

    public int? UserId { get; set; }

    public DateTime? LastPlayed { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Track? Track { get; set; }

    public virtual User? User { get; set; }
}
