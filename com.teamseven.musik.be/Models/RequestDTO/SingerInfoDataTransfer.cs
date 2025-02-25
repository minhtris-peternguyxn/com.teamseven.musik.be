namespace com.teamseven.musik.be.Models.RequestDTO
{
    public class SingerInfoDataTransfer
    {
        public int ArtistId { get; set; }

        public string? ArtistName { get; set; }

        public int? VerifiedArtist { get; set; }

        public int? SubscribeNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Img { get; set; }

    }
}
