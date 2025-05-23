﻿namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class AlbumRequest
    {
        public string? AlbumName { get; set; }

        public required List<int> ArtistIds { get; set; }

        public required List<int> TrackIds { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string? Img { get; set; }
    }
}
