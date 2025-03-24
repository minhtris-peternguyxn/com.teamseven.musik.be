namespace com.teamseven.musik.be.Models.Entities;

public partial class TrackAlbum
{
    public TrackAlbum() { }
    public TrackAlbum(int trackId, int albumId)
    {
        TrackId = trackId;
        AlbumId = albumId;
    }

    public int TrackId { get; set; }
    public int AlbumId { get; set; }

    // Navigation properties
    public Track Track { get; set; }
    public Album Album { get; set; }
}