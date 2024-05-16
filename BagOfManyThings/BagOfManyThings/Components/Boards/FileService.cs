using BagOfManyThings.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace BagOfManyThings.Components.Boards
{
    public class FileService
    {

        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public FileService(IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _env = env;
            _userManager = userManager;
        }

        public string GetUserDirectoryPath(string userId)
        {
            var basePath = GetBaseDirectoryPath();
            var userDirectory = Path.Combine(basePath, userId);
            return userDirectory;
        }
        public string GetBaseDirectoryPath()
        {
            return  Path.Combine(_env.ContentRootPath, "DMData");
        }
        public string GetRootDirectoryPath()
        {
            return _env.ContentRootPath;
        }

        public void CreateUserDirectory(string userId) //creates a UserDirectory in DMData (Create Campaign basically)
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);
            Directory.CreateDirectory(userDirectoryPath);
        }
        public void CreateFolderInUserDirectory(string userId, string folderName)//creates a folder in the userId thing 
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);
            var folderPath = Path.Combine(userDirectoryPath, folderName);
            Directory.CreateDirectory(userDirectoryPath);
        }

        public async Task CreateFileInDirectoryAsync(string directoryPath, string fileName, string content)
        {
            if (fileName == null)
                throw new ArgumentNullException("FileName is null");
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            var filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
                throw new IOException($"File already exists: {filePath}");

            await File.WriteAllTextAsync(filePath, content);
        }
        public void CreateFolderInDirectory(string directoryPath, string folderName)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory doesn't exists.");
                return;
            }
            if(Directory.Exists(Path.Combine(directoryPath,folderName))) { Console.WriteLine("Directory already exists.");
                return;
            }
            string folderPath = Path.Combine(directoryPath, folderName);

            if (Directory.Exists(folderPath))
                throw new IOException($"Folder already exists: {folderPath}");

            Directory.CreateDirectory(folderPath);
        }

        public async Task CreateMarkdownFileAsync(string filePath, string fileName)//creates a Markdown file
        {
            filePath = Path.Combine(filePath, fileName + ".md");
            await File.WriteAllTextAsync(filePath, null);
        }
        public void DeleteDirectory(string basePath) //deletes directory
        {
            Directory.Delete(basePath, true);
        }
    }
}