using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;

    namespace BagOfManyThings.Components.Boards
    {
        public class FileSystem
    {
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;

        public FileSystem(IWebHostEnvironment env, UserManager<IdentityUser> userManager)
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

        public void CreateUserDirectory(string userId)
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);
            Directory.CreateDirectory(userDirectoryPath);
        }

        public async Task CreateFileAsync(string userId, string fileName, string content)
        {
            var userDirectoryPath = GetUserDirectoryPath(userId);
            var filePath = Path.Combine(userDirectoryPath, fileName + ".md");
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}