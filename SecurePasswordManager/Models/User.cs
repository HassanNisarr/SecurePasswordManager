using System;
using System.Collections.Generic;

namespace SecurePasswordManager.Models;

public partial class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public virtual ICollection<PasswordRecord> PasswordRecords { get; set; } = new List<PasswordRecord>();
}
