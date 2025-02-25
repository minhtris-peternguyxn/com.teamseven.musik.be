using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Podcast
{
    public int PodcastId { get; set; }

    public int? ArtistId { get; set; }

    public int? PlaylistId { get; set; }

    public string? PodcastTitle { get; set; }

    public string? PodcastDetail { get; set; }

    public int? Duration { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual Playlist? Playlist { get; set; }
}
