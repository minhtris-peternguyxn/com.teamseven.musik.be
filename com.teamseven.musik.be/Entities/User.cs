﻿using System;
using System.Collections.Generic;

namespace com.teamseven.musik.be.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Address { get; set; }

    public string AccountType { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
