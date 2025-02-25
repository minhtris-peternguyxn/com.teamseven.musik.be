using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Album
{
    public int AlbumId { get; set; }

    public string? AlbumName { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public int? ArtistId { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
