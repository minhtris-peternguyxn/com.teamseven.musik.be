using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Album
{
    public int AlbumId { get; set; }
    public string AlbumName { get; set; }
    public string? NormalizedName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Img { get; set; }

    // Navigation properties
    public ICollection<TrackAlbum> TrackAlbums { get; set; } = new List<TrackAlbum>();
    public ICollection<AlbumArtist> AlbumArtists { get; set; } = new List<AlbumArtist>();
}