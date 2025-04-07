using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Track
{
    public int TrackId { get; set; }
    public string TrackName { get; set; }

    public string? NormalizedName { get; set; }
    public int Duration { get; set; }
    public string Img { get; set; }
    public string TrackBlobsLink { get; set; }
    public DateTime CreatedDate { get; set; }
    public int TotalLikes { get; set; }
    public int TotalViews { get; set; }

    // Navigation properties
    public ICollection<TrackGenre> TrackGenres { get; set; } = new List<TrackGenre>();
    public ICollection<TrackAlbum> TrackAlbums { get; set; } = new List<TrackAlbum>();
    public ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    public ICollection<History> Histories { get; set; } = new List<History>();
}