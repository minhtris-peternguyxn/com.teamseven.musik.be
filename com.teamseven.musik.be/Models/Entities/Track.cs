using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Track
{
    public int TrackId { get; set; }

    public string? TrackName { get; set; }

    public int? Duration { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public long? TotalViews { get; set; }

    public int? TotalLikes { get; set; }

    public string? TrackBlobsLink { get; set; }
}
