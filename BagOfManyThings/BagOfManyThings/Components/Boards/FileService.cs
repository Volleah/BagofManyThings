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
            var basePath = Path.Combine(_env.ContentRootPath, "DMData");
            var userDirectory = Path.Combine(basePath, userId);
            return userDirectory;
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

        public async Task CreateMarkdownFileAsync(string userId, string fileName, string content)//creates a Markdown file
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);
            var filePath = Path.Combine(userDirectoryPath, fileName + ".md");
            await File.WriteAllTextAsync(filePath, content);
        }
        public void DeleteUserDirectory(string userId) //deletes campaign
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);

            if (Directory.Exists(userDirectoryPath))
            {
                Directory.Delete(userDirectoryPath, true);
            }
        }
    }
}