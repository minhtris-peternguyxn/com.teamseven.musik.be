namespace com.teamseven.musik.be.Models.Entities;

public partial class TrackGenre
{
    public TrackGenre() { }
    public TrackGenre(int trackId, int genreId)
    {
        TrackId = trackId;
        GenreId = genreId;
    }

    public int TrackId { get; set; }
    public int GenreId { get; set; }

    // Navigation properties
    public Track Track { get; set; }
    public Genre Genre { get; set; }
}