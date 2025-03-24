namespace com.teamseven.musik.be.Models.Entities;

public partial class PlaylistTrack
{
    public PlaylistTrack() { }
    public PlaylistTrack(int playlistId, int trackId)
    {
        PlaylistId = playlistId;
        TrackId = trackId;
    }

    public int PlaylistId { get; set; }
    public int TrackId { get; set; }

    // Navigation properties
    public Playlist Playlist { get; set; }
    public Track Track { get; set; }
}