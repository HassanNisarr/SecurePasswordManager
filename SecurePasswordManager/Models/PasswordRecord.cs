using System;
using System.Collections.Generic;

namespace SecurePasswordManager.Models;

public partial class PasswordRecord
{
    public int RecordId { get; set; }
    public string Username { get; set; }
    public string PlatformName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? AdditionalInfo { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public virtual User User { get; set; } = null!;
}
