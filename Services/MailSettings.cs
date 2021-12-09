// 5.1 Add MailSettings in appsettings.json
// 5.2 Create class MailSettings
public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}