using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class TrackAlbum
{
    public TrackAlbum(int trackId, int albumId) 
    {
        TrackId = trackId;
        AlbumId = albumId;
    }

    public int TrackId { get; set; }

    public int AlbumId { get; set; }
}

