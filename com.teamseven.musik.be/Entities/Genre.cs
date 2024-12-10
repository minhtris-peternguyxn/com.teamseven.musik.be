using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
