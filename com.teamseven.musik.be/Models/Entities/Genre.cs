using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; }

    public string? NormalizedName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Img { get; set; }

    // Navigation properties
    public ICollection<TrackGenre> TrackGenres { get; set; } = new List<TrackGenre>();
    public ICollection<History> Histories { get; set; } = new List<History>();
}