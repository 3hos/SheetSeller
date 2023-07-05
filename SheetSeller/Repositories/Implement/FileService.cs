using SheetSeller.Repositories.Abstract;
using SheetSeller.Models.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SheetSeller.Repositories.Implement
{
    public class FileService : IFileService
    {
        private readonly string path;
        public FileService(IWebHostEnvironment env)
        {
            var wwwPath = env.WebRootPath;
            var dir = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(path);
            }
            this.path = dir;
        }
        public async Task<Status> SaveImageAsync(IFormFile imageFile, string ID)
        {
            try
            {
                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Status(){ Message = msg,StatusCode=0 };
                };
                using var image = Image.Load(imageFile.OpenReadStream());
                if (image.Height > image.Width)
                {
                    image.Mutate(x => x.Crop(image.Width, image.Height - (image.Height - image.Width) / 2));
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                    image.Mutate(x => x.Crop(image.Width, image.Width));
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                else if (image.Width > image.Height)
                {
                    image.Mutate(x => x.Crop(image.Height, image.Width - (image.Width - image.Height) / 2));
                    image.Mutate(x => x.Flip(FlipMode.Horizontal));
                    image.Mutate(x => x.Crop(image.Height, image.Height));
                    image.Mutate(x => x.Flip(FlipMode.Horizontal));
                }
                image.Mutate(x => x.Resize(512, 512));
                var FileName = ID + ext;
                var fileWithPath = Path.Combine(path, FileName);
                await image.SaveAsync(fileWithPath);
                return new Status(){ StatusCode=1,Message=FileName};
            }
            catch
            {
                return new Status() { StatusCode=0,Message= "Can`t save your iage" };
            }
        }

        public Status DeleteFile(string FileName)
        {
            try
            {
                var fileWithPath = Path.Combine(path, FileName);
                if (File.Exists(fileWithPath))
                {
                    File.Delete(fileWithPath);
                    return new Status() { StatusCode = 1, Message = "Deleted succes" };
                }
                return new Status() { StatusCode = 0, Message = "File not found" };
            }
            catch 
            {
                return new Status() { StatusCode = 0, Message = " Some error occured" };
            }
        }
        public Status SavePDF(IFormFile File, string ID)
        {
            try
            {
                // Check the allowed extenstions
                var ext = Path.GetExtension(File.FileName);
                if (ext!=".pdf")
                {
                    string msg = string.Format("Only PDF files are allowed");
                    return new Status() { Message = msg, StatusCode = 0 };
                };
                var FileName = ID + ext;
                var fileWithPath = Path.Combine(path, FileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                File.CopyTo(stream);
                stream.Close();
                return new Status() { StatusCode = 1, Message = FileName };
            }
            catch
            {
                return new Status() { StatusCode = 0, Message = "Can`t save your iage" };
            }
        }
    }
}
