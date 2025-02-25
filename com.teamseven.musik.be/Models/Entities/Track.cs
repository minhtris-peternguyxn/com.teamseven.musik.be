using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Track
{
    public int TrackId { get; set; }

    public string? TrackName { get; set; }

    public int? Duration { get; set; }

    public int? ArtistId { get; set; }

    public int? AlbumId { get; set; }

    public int? GenreId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public long? TotalViews { get; set; }

    public int? TotalLikes { get; set; }

    public string? TrackBlobsLink { get; set; }

    public virtual Album? Album { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Genre? Genre { get; set; }
}
