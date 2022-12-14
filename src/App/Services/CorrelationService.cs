namespace App.Services;

public class CorrelationService : ICorrelationService
{
    private readonly string _correlationId;

    public CorrelationService()
    {
        _correlationId = Guid.NewGuid().ToString("D");
    }

    public string GetCorrelationId() => _correlationId;
}