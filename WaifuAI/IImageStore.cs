namespace Waifustasia.WaifuAI
{
    public interface IImageStore
    {
        Task<string> SaveImageAsync(string url);
    }
}