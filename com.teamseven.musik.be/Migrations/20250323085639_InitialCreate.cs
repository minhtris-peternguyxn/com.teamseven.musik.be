using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.teamseven.musik.be.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    album_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    album_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    release_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    img = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Album__B0E1DDB2EF372859", x => x.album_id);
                });

            migrationBuilder.CreateTable(
                name: "Artist",
                columns: table => new
                {
                    artist_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    img = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    subscribe_number = table.Column<int>(type: "int", nullable: false),
                    verified_artist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Artist__6CD04001AD589F42", x => x.artist_id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    genre_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genre_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    img = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Genre__18428D42BE113DBB", x => x.genre_id);
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    track_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    duration = table.Column<int>(type: "int", nullable: false),
                    img = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    track_blobs_link = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    total_likes = table.Column<int>(type: "int", nullable: false),
                    total_views = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Track__24ECC82EC89450EE", x => x.track_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    account_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    img_link = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    number_of_subscriber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__B9BE370F8C11AB72", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "AlbumArtist",
                columns: table => new
                {
                    album_id = table.Column<int>(type: "int", nullable: false),
                    artist_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AlbumArt__B62CD9B2588E67DA", x => new { x.album_id, x.artist_id });
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Album",
                        column: x => x.album_id,
                        principalTable: "Album",
                        principalColumn: "album_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumArtist_Artist",
                        column: x => x.artist_id,
                        principalTable: "Artist",
                        principalColumn: "artist_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackAlbum",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "int", nullable: false),
                    album_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrackAlb__1FE2D5F56620156E", x => new { x.track_id, x.album_id });
                    table.ForeignKey(
                        name: "FK_TrackAlbum_Album",
                        column: x => x.album_id,
                        principalTable: "Album",
                        principalColumn: "album_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackAlbum_Track",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtist",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "int", nullable: false),
                    artist_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrackArt__2221CC2E9FA568C4", x => new { x.track_id, x.artist_id });
                    table.ForeignKey(
                        name: "FK_TrackArtist_Artist",
                        column: x => x.artist_id,
                        principalTable: "Artist",
                        principalColumn: "artist_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackArtist_Track",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackGenre",
                columns: table => new
                {
                    track_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TrackGen__1568E0FAE821F8B0", x => new { x.track_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_TrackGenre_Genre",
                        column: x => x.genre_id,
                        principalTable: "Genre",
                        principalColumn: "genre_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackGenre_Track",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    track_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    last_played = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    genre_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__History__096AA2E939D6FE0E", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_History_Genre",
                        column: x => x.genre_id,
                        principalTable: "Genre",
                        principalColumn: "genre_id");
                    table.ForeignKey(
                        name: "FK_History_Track",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id");
                    table.ForeignKey(
                        name: "FK_History_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Playlist",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    playlist_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Playlist__FB9C14108B86F8A5", x => x.playlist_id);
                    table.ForeignKey(
                        name: "FK_Playlist_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTrack",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "int", nullable: false),
                    track_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Playlist__09D2D892E3577A98", x => new { x.playlist_id, x.track_id });
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Playlist",
                        column: x => x.playlist_id,
                        principalTable: "Playlist",
                        principalColumn: "playlist_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Track",
                        column: x => x.track_id,
                        principalTable: "Track",
                        principalColumn: "track_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Podcast",
                columns: table => new
                {
                    podcast_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_id = table.Column<int>(type: "int", nullable: true),
                    playlist_id = table.Column<int>(type: "int", nullable: true),
                    podcast_title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    podcast_detail = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    release_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    img = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Podcast__147CC04273B6550C", x => x.podcast_id);
                    table.ForeignKey(
                        name: "FK_Podcast_Artist",
                        column: x => x.artist_id,
                        principalTable: "Artist",
                        principalColumn: "artist_id");
                    table.ForeignKey(
                        name: "FK_Podcast_Playlist",
                        column: x => x.playlist_id,
                        principalTable: "Playlist",
                        principalColumn: "playlist_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtist_artist_id",
                table: "AlbumArtist",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_History_genre_id",
                table: "History",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_History_track_id",
                table: "History",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_History_user_id",
                table: "History",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Playlist_user_id",
                table: "Playlist",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_track_id",
                table: "PlaylistTrack",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_Podcast_artist_id",
                table: "Podcast",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_Podcast_playlist_id",
                table: "Podcast",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackAlbum_album_id",
                table: "TrackAlbum",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtist_artist_id",
                table: "TrackArtist",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackGenre_genre_id",
                table: "TrackGenre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "UQ_User_Email",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumArtist");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "PlaylistTrack");

            migrationBuilder.DropTable(
                name: "Podcast");

            migrationBuilder.DropTable(
                name: "TrackAlbum");

            migrationBuilder.DropTable(
                name: "TrackArtist");

            migrationBuilder.DropTable(
                name: "TrackGenre");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
