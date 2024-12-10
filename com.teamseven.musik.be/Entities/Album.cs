using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class Album
{
    public int AlbumId { get; set; }

    public string AlbumName { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
