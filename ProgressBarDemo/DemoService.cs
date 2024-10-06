namespace ProgressBarDemo;

public static class DemoService
{
    public static async Task DemoAsync(int begin, int end, CancellationToken? token = null, IProgress<int>? progress = null)
    {
        for (int i = begin; i <= end; i++)
        {
            if (token.HasValue && token.Value.IsCancellationRequested)
            {
                return;
            }
            
            progress?.Report(i);

            await Task.Delay(500);
        }
    }
}