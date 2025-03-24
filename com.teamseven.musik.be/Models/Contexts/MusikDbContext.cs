using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Models.Contexts;

public partial class MusikDbContext : DbContext
{
    public MusikDbContext() { }

    public MusikDbContext(DbContextOptions<MusikDbContext> options) : base(options) { }

    // DbSet cho bảng chính
    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Artist> Artists { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<History> Histories { get; set; }
    public virtual DbSet<Playlist> Playlists { get; set; }
    public virtual DbSet<Podcast> Podcasts { get; set; }
    public virtual DbSet<Track> Tracks { get; set; }
    public virtual DbSet<User> Users { get; set; }

    // DbSet cho bảng nhiều-nhiều
    public virtual DbSet<TrackGenre> TrackGenres { get; set; }
    public virtual DbSet<TrackAlbum> TrackAlbums { get; set; }
    public virtual DbSet<TrackArtist> TrackArtists { get; set; }
    public virtual DbSet<AlbumArtist> AlbumArtists { get; set; }
    public virtual DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Album
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("PK__Album__B0E1DDB2EF372859");
            entity.ToTable("Album");

            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.AlbumName).HasMaxLength(255).HasColumnName("album_name"); // Đổi sang nvarchar
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.Img).HasMaxLength(255).HasColumnName("img"); // Đổi sang nvarchar
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime").HasColumnName("release_date");
        });

        // Artist
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PK__Artist__6CD04001AD589F42");
            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.ArtistName).HasMaxLength(255).HasColumnName("artist_name"); // Đổi sang nvarchar
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.Img).HasMaxLength(255).HasColumnName("img"); // Đổi sang nvarchar
            entity.Property(e => e.SubscribeNumber).HasColumnName("subscribe_number");
            entity.Property(e => e.VerifiedArtist).HasColumnName("verified_artist");
        });

        // Genre
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__18428D42BE113DBB");
            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.GenreName).HasMaxLength(255).HasColumnName("genre_name"); // Đổi sang nvarchar
            entity.Property(e => e.Img).HasMaxLength(255).HasColumnName("img"); // Đổi sang nvarchar
        });

        // History
        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__History__096AA2E939D6FE0E");
            entity.ToTable("History");

            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.LastPlayed).HasColumnType("datetime").HasColumnName("last_played");
            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.Histories).HasForeignKey(d => d.GenreId).HasConstraintName("FK_History_Genre");
            entity.HasOne(d => d.Track).WithMany(p => p.Histories).HasForeignKey(d => d.TrackId).HasConstraintName("FK_History_Track");
            entity.HasOne(d => d.User).WithMany(p => p.Histories).HasForeignKey(d => d.UserId).HasConstraintName("FK_History_User");
        });

        // Playlist
        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PK__Playlist__FB9C14108B86F8A5");
            entity.ToTable("Playlist");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.PlaylistName).HasMaxLength(255).HasColumnName("playlist_name"); // Đổi sang nvarchar
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists).HasForeignKey(d => d.UserId).HasConstraintName("FK_Playlist_User");
        });

        // Podcast
        modelBuilder.Entity<Podcast>(entity =>
        {
            entity.HasKey(e => e.PodcastId).HasName("PK__Podcast__147CC04273B6550C");
            entity.ToTable("Podcast");

            entity.Property(e => e.PodcastId).HasColumnName("podcast_id");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Img).HasMaxLength(255).HasColumnName("img"); // Đổi sang nvarchar
            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.PodcastDetail).HasMaxLength(500).HasColumnName("podcast_detail"); // Đổi sang nvarchar
            entity.Property(e => e.PodcastTitle).HasMaxLength(255).HasColumnName("podcast_title"); // Đổi sang nvarchar
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime").HasColumnName("release_date");

            entity.HasOne(d => d.Artist).WithMany(p => p.Podcasts).HasForeignKey(d => d.ArtistId).HasConstraintName("FK_Podcast_Artist");
            entity.HasOne(d => d.Playlist).WithMany(p => p.Podcasts).HasForeignKey(d => d.PlaylistId).HasConstraintName("FK_Podcast_Playlist");
        });

        // Track
        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("PK__Track__24ECC82EC89450EE");
            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Img).HasMaxLength(255).HasColumnName("img"); // Đổi sang nvarchar
            entity.Property(e => e.TotalLikes).HasColumnName("total_likes");
            entity.Property(e => e.TotalViews).HasColumnName("total_views");
            entity.Property(e => e.TrackBlobsLink).HasMaxLength(255).HasColumnName("track_blobs_link"); // Đổi sang nvarchar
            entity.Property(e => e.TrackName).HasMaxLength(255).HasColumnName("track_name"); // Đổi sang nvarchar
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F8C11AB72");
            entity.ToTable("User");
            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AccountType).HasMaxLength(50).HasColumnName("account_type"); // Đổi sang nvarchar
            entity.Property(e => e.Address).HasMaxLength(255).HasColumnName("address"); // Đổi sang nvarchar
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime").HasColumnName("created_at");
            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email"); // Đổi sang nvarchar
            entity.Property(e => e.ImgLink).HasMaxLength(255).HasColumnName("img_link"); // Đổi sang nvarchar
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name"); // Đổi sang nvarchar
            entity.Property(e => e.NumberOfSubscriber).HasColumnName("number_of_subscriber");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password"); // Đổi sang nvarchar
            entity.Property(e => e.PhoneNumber).HasMaxLength(15).HasColumnName("phone_number"); // Đổi sang nvarchar
            entity.Property(e => e.Role).HasMaxLength(50).HasColumnName("role"); // Đổi sang nvarchar
        });

        // Cấu hình bảng nhiều-nhiều
        // TrackGenre
        modelBuilder.Entity<TrackGenre>(entity =>
        {
            entity.HasKey(tg => new { tg.TrackId, tg.GenreId }).HasName("PK__TrackGen__1568E0FAE821F8B0");
            entity.ToTable("TrackGenre");

            entity.Property(tg => tg.TrackId).HasColumnName("track_id");
            entity.Property(tg => tg.GenreId).HasColumnName("genre_id");

            entity.HasOne(tg => tg.Track).WithMany(t => t.TrackGenres).HasForeignKey(tg => tg.TrackId).HasConstraintName("FK_TrackGenre_Track");
            entity.HasOne(tg => tg.Genre).WithMany(g => g.TrackGenres).HasForeignKey(tg => tg.GenreId).HasConstraintName("FK_TrackGenre_Genre");
        });

        // TrackAlbum
        modelBuilder.Entity<TrackAlbum>(entity =>
        {
            entity.HasKey(ta => new { ta.TrackId, ta.AlbumId }).HasName("PK__TrackAlb__1FE2D5F56620156E");
            entity.ToTable("TrackAlbum");

            entity.Property(ta => ta.TrackId).HasColumnName("track_id");
            entity.Property(ta => ta.AlbumId).HasColumnName("album_id");

            entity.HasOne(ta => ta.Track).WithMany(t => t.TrackAlbums).HasForeignKey(ta => ta.TrackId).HasConstraintName("FK_TrackAlbum_Track");
            entity.HasOne(ta => ta.Album).WithMany(a => a.TrackAlbums).HasForeignKey(ta => ta.AlbumId).HasConstraintName("FK_TrackAlbum_Album");
        });

        // TrackArtist
        modelBuilder.Entity<TrackArtist>(entity =>
        {
            entity.HasKey(ta => new { ta.TrackId, ta.ArtistId }).HasName("PK__TrackArt__2221CC2E9FA568C4");
            entity.ToTable("TrackArtist");

            entity.Property(ta => ta.TrackId).HasColumnName("track_id");
            entity.Property(ta => ta.ArtistId).HasColumnName("artist_id");

            entity.HasOne(ta => ta.Track).WithMany(t => t.TrackArtists).HasForeignKey(ta => ta.TrackId).HasConstraintName("FK_TrackArtist_Track");
            entity.HasOne(ta => ta.Artist).WithMany(a => a.TrackArtists).HasForeignKey(ta => ta.ArtistId).HasConstraintName("FK_TrackArtist_Artist");
        });

        // AlbumArtist
        modelBuilder.Entity<AlbumArtist>(entity =>
        {
            entity.HasKey(aa => new { aa.AlbumId, aa.ArtistId }).HasName("PK__AlbumArt__B62CD9B2588E67DA");
            entity.ToTable("AlbumArtist");

            entity.Property(aa => aa.AlbumId).HasColumnName("album_id");
            entity.Property(aa => aa.ArtistId).HasColumnName("artist_id");

            entity.HasOne(aa => aa.Album).WithMany(a => a.AlbumArtists).HasForeignKey(aa => aa.AlbumId).HasConstraintName("FK_AlbumArtist_Album");
            entity.HasOne(aa => aa.Artist).WithMany(a => a.AlbumArtists).HasForeignKey(aa => aa.ArtistId).HasConstraintName("FK_AlbumArtist_Artist");
        });

        // PlaylistTrack
        modelBuilder.Entity<PlaylistTrack>(entity =>
        {
            entity.HasKey(pt => new { pt.PlaylistId, pt.TrackId }).HasName("PK__Playlist__09D2D892E3577A98");
            entity.ToTable("PlaylistTrack");

            entity.Property(pt => pt.PlaylistId).HasColumnName("playlist_id");
            entity.Property(pt => pt.TrackId).HasColumnName("track_id");

            entity.HasOne(pt => pt.Playlist).WithMany(p => p.PlaylistTracks).HasForeignKey(pt => pt.PlaylistId).HasConstraintName("FK_PlaylistTrack_Playlist");
            entity.HasOne(pt => pt.Track).WithMany(t => t.PlaylistTracks).HasForeignKey(pt => pt.TrackId).HasConstraintName("FK_PlaylistTrack_Track");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}