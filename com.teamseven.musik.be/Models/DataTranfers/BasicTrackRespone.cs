namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class BasicTrackRespone
    {
        public int TrackId { get; set; }
        public string? TrackName { get; set; }
        public string? TrackBlobsLink { get; set; }
        public int? Duration { get; set; }
        public List<string>? Artists { get; set; }

        public string? Img { get; set; }
    }
}
