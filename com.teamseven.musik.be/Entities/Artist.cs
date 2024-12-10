using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public bool VerifiedArtist { get; set; }

    public int SubscribeNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
