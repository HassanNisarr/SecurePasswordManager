namespace SecurePasswordManager.Models
{
    public class UpdatePasswordRecordViewModel
    {
        public int RecordId { get; set; }
        public string Username { get; set; }
        public string PlatformName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AdditionalInfo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
