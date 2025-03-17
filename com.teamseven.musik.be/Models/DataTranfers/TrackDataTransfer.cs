namespace com.teamseven.musik.be.Models.Request
{
    public class TrackDataTransfer
    {

        public int TrackId { get; set; }
        public string? TrackName { get; set; }
        public string? TrackBlobsLink { get; set; }
        public int? Duration { get; set; }
        public List<int>? ArtistIds { get; set; }

        public List<int>? GenreIds { get; set; }

        public List<int>? AlbumIds { get; set; }

        public string? Img { get; set; }
    }
}