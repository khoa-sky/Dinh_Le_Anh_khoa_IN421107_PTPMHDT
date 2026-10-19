namespace EduHub.Ch02.Notifications;

public class NotificationServiceFactory : INotificationServiceFactoty
{
    public INotificationService Create(NotificationChannel channel) => channel switch
    {
        NotificationChannel.Email => new EmailNotificationService(),
        NotificationChannel.Sms => new SmsNotificationService(),
        NotificationChannel.Slack => new SlackNotificationService(),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Unsupported channel")
    };
}