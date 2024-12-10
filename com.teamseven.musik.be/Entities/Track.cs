using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; } = null!;

    public int Duration { get; set; }

    public int ArtistId { get; set; }

    public int AlbumId { get; set; }

    public int GenreId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Artist Artist { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
