using System.IO;

namespace _3D_Engine.Loaders
{
    public class Loader
    {
        internal void FileExistCheck(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // raise a custom exception
            }
        }
    }
}