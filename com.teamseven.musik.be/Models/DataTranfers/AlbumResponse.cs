namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class AlbumResponse
    {
        public int Id { get; set; }

        public string? AlbumName { get; set; }

        public  List<int> ArtistIds { get; set; }

        public  List<int> TrackIds { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string? Img { get; set; }
    }
}
