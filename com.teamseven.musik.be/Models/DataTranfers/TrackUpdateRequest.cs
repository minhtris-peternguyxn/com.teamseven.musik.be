namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class TrackUpdateRequest
    {
        public int TrackId { get; set; }

        public string? TrackName { get; set; }

        public int? Duration { get; set; }

        public string? Img { get; set; }

        public string? TrackBlobsLink { get; set; }
    }
}
