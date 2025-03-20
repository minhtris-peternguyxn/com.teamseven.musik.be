namespace com.teamseven.musik.be.Models.DataTranfers
{
    public class TrackCreateRequest
    {
        public string? TrackName { get; set; }

        public int? Duration { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Img { get; set; }

        public string? TrackBlobsLink { get; set; }
    }
}
