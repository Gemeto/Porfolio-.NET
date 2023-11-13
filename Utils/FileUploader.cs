using PorfolioWeb.Models.Interfaces;

namespace PorfolioWeb.Widgets
{
    public class FileUploader<T> where T : IFileModel, new()
    {
        private string _fileName;
        private string _path;
        IFormFile _file;

        public FileUploader(IFormFile file, string fileName, string path)
        {
            _fileName = fileName;
            _path = path;
            _file = file;
        }

        public async Task<IFileModel?> UploadFileAsync()
        {
            if (_fileName != null && _path != null)
            {
                await _file.CopyToAsync(new FileStream(_path, FileMode.Create));
                T fileModel = new T
                {
                    Path = $"/uploads/{_fileName}"
                };
                return fileModel;
            }
            return null;
        }
    }
}
