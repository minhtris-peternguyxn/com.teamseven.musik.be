using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? ArtistName { get; set; }

    public int? VerifiedArtist { get; set; }

    public int? SubscribeNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
