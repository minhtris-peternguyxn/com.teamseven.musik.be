using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string? PlaylistName { get; set; }

    public int? UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Podcast> Podcasts { get; set; } = new List<Podcast>();

    public virtual User? User { get; set; }
}
