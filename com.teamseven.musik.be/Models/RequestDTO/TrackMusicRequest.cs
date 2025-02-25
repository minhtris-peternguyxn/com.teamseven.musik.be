namespace com.teamseven.musik.be.Models.Request
{
    public class TrackMusicRequest
    {
        public string? TrackName { get; set; }
        public string? TrackBlobsLink { get; set; }
        public int? Duration { get; set; }
        public int? ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public int? GenreId { get; set; }
        public string? Img { get; set; }
    }
}