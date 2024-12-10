using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class Podcast
{
    public int PodcastId { get; set; }

    public int ArtistId { get; set; }

    public int PlaylistId { get; set; }

    public string PodcastTitle { get; set; } = null!;

    public string PodcastDetail { get; set; } = null!;

    public int Duration { get; set; }

    public DateTime ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual Playlist Playlist { get; set; } = null!;
}
