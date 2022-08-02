namespace tiki_shop.Services.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);
        Task<string> SaveFileAsync(IFormFile image);
        Task DeleteFileAsync(string fileName);
        string GetFolder();
        string GetPath(string filename);
    }
}
