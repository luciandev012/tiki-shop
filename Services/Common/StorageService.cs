using System.Net.Http.Headers;


namespace tiki_shop.Services.Common
{
    public class StorageService : IStorageService
    {
        private const string contentFolderName = "\\uploads\\";
        private readonly string _contentFolder;
        public StorageService(IWebHostEnvironment webHostEnvironment)
        {
            _contentFolder = webHostEnvironment.WebRootPath + contentFolderName;
        }

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFolder()
        {
            throw new NotImplementedException();
        }

        public string GetPath(string filename)
        {
            return Path.Combine(_contentFolder, filename);
        }

        public async Task<string> SaveFileAsync(IFormFile image)
        {
            if(!Directory.Exists(_contentFolder))
            {
                Directory.CreateDirectory(_contentFolder);
            }
            var originalFileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Models.Common.Common.ConvertToUnSign(originalFileName)}";
            var filePath = Path.Combine(_contentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await image.OpenReadStream().CopyToAsync(output);
            return fileName;
        }
        
    }
}
