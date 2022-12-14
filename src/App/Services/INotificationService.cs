namespace App.Services;

public interface INotificationService
{
    Task SendAsync(Notification notification, CancellationToken cancellationToken = default);
}