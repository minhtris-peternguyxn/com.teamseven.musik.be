using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class AlbumArtist
{
    public AlbumArtist(int albumId, int artistId)
    {
        AlbumId = albumId;
        ArtistId = artistId;
    }

    public int AlbumId { get; set; }

    public int ArtistId { get; set; }
}
