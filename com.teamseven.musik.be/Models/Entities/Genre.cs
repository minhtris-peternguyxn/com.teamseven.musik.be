using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? GenreName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<History> Histories { get; set; } = new List<History>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
