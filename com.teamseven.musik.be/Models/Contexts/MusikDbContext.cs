using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Models.Contexts;

public partial class MusikDbContext : DbContext
{
    public MusikDbContext()
    {
    }

    public MusikDbContext(DbContextOptions<MusikDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Genre> Genre { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Podcast> Podcasts { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("PK__Album__B0E1DDB2D900B5A9");

            entity.ToTable("Album");

            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.AlbumName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("album_name");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("release_date");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_Album_Artist");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PK__Artist__6CD0400149268228");

            entity.ToTable("Artist");

            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.ArtistName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("artist_name");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.SubscribeNumber).HasColumnName("subscribe_number");
            entity.Property(e => e.VerifiedArtist).HasColumnName("verified_artist");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__18428D42FD526453");

            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GenreName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("genre_name");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__History__096AA2E964F19230");

            entity.ToTable("History");

            entity.Property(e => e.HistoryId).HasColumnName("history_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.LastPlayed)
                .HasColumnType("datetime")
                .HasColumnName("last_played");
            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Genre).WithMany(p => p.Histories)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_History_Genre");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PK__Playlist__FB9C1410729C7F6E");

            entity.ToTable("Playlist");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.PlaylistName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("playlist_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Playlist__user_i__4F7CD00D");
        });

        modelBuilder.Entity<Podcast>(entity =>
        {
            entity.HasKey(e => e.PodcastId).HasName("PK__Podcast__147CC0429BD514C6");

            entity.ToTable("Podcast");

            entity.Property(e => e.PodcastId).HasColumnName("podcast_id");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.PodcastDetail)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("podcast_detail");
            entity.Property(e => e.PodcastTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("podcast_title");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("release_date");

            entity.HasOne(d => d.Artist).WithMany(p => p.Podcasts)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK__Podcast__artist___5070F446");

            entity.HasOne(d => d.Playlist).WithMany(p => p.Podcasts)
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("FK__Podcast__playlis__5165187F");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("PK__Track__24ECC82E14433D93");

            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.TotalLikes).HasColumnName("total_likes");
            entity.Property(e => e.TotalViews).HasColumnName("total_views");
            entity.Property(e => e.TrackBlobsLink)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("track_blobs_link");
            entity.Property(e => e.TrackName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("track_name");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("FK__Track__album_id__52593CB8");

            entity.HasOne(d => d.Artist).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK__Track__artist_id__534D60F1");

            entity.HasOne(d => d.Genre).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Track__genre_id__5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F55B0FBD8");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164A8CEAE57").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("account_type");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.ImgLink)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img_link");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfSubscriber).HasColumnName("number_of_subscriber");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
