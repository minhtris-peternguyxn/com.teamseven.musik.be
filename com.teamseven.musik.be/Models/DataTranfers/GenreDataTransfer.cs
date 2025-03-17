namespace com.teamseven.musik.be.Models.RequestDTO
{
    public class GenreDataTransfer
    {
        public int GenreId { get; set; }

        public string? GenreName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Img { get; set; }

    }
}
