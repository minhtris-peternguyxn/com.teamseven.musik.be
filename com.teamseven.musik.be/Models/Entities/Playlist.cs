using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Playlist
{
    public int PlaylistId { get; set; }
    public string PlaylistName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    public ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();
}