namespace SecurePasswordManager.Models
{
    public class AddPasswordRecordViewModel
    {
        //public int RecordId { get; set; }
        public string PlatformName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AdditionalInfo { get; set; }

    }
}
