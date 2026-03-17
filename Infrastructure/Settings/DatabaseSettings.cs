namespace Infrastructure.Settings;

public record DatabaseSettings
{
  public string Path { get; init; } = string.Empty;
}
