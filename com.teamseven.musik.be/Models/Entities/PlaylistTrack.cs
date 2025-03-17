using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class PlaylistTrack
{
    public int PlaylistId { get; set; }

    public int TrackId { get; set; }
}
