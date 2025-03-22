﻿namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class TrackCreateRequest
    {
        public string? TrackName { get; set; }

        public int? Duration { get; set; }

        public string? Img { get; set; }

        public string? TrackBlobsLink { get; set; }

        public List<int> ArtistIds { get; set; }

        public List<int> GenresIds { get; set; }
    }
}
