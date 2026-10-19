namespace EduHub.Ch02.Notifications;

public interface INotificationServiceFactoty
{
    INotificationService Create(NotificationChannel channel);
}