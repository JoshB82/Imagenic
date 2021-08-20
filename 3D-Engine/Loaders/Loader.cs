using _3D_Engine.Constants;
using System.IO;

namespace _3D_Engine.Loaders
{
    public class Loader
    {
        #region Fields and Properties

        public string FilePath { get; set; }

        #endregion

        #region Constructors

        public Loader(string filePath)
        {
            FilePath = filePath;
            FileExistCheck(filePath);
        }

        #endregion

        #region Methods

        internal void FileExistCheck(string filePath)
        {
            if (!File.Exists(filePath))
            {
                GenerateException.GenerateException<FileDoesNotExistException>(filePath);
            }
        }

        #endregion
    }
}