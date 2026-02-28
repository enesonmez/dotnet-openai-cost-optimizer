namespace DotNetOpenAICostOptimizer.Services;

public interface IOpenAiService
{
    Task<string> AskAsync(string prompt, CancellationToken token = default);
}