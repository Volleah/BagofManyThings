namespace BagOfManyThings.Components.Boards
{
    public class FileSystemItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }

        public FileSystemItem(string name, string path, bool isDirectory)
        {
            Name = name;
            Path = path;
            IsDirectory = isDirectory;
        }
    }
}
