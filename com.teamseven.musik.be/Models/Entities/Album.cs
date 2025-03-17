using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class Album
{
    public int AlbumId { get; set; }

    public string? AlbumName { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Img { get; set; }
}
