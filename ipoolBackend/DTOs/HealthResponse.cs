namespace ipoolBackend.DTOs;

public record HealthResponse(string Status, bool Database, DateTime TimestampUtc);
