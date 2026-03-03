namespace DotNetOpenAICostOptimizer.Services;

public interface IAiProvider
{
    Task<string> AskAsync(string prompt, CancellationToken token = default);
}