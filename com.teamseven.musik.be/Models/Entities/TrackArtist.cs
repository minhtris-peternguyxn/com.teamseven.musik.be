using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class TrackArtist
{
    public TrackArtist(int trackId, int artistId)
    {
        TrackId = trackId;
        ArtistId = artistId;
    }

    public int TrackId { get; set; }

    public int ArtistId { get; set; }
}
