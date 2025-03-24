using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? AccountType { get; set; }

    public string? ImgLink { get; set; }

    public int? NumberOfSubscriber { get; set; }

    public virtual ICollection<History> Histories { get; set; } = new List<History>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
