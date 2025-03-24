using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Artist
{
    public int ArtistId { get; set; }
    public string ArtistName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Img { get; set; }
    public int SubscribeNumber { get; set; }
    public bool VerifiedArtist { get; set; }

    // Navigation properties
    public ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
    public ICollection<AlbumArtist> AlbumArtists { get; set; } = new List<AlbumArtist>();
    public ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();
}