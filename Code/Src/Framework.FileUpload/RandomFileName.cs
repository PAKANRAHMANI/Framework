using System;
using System.IO;
using System.Text;

namespace Framework.FileUpload
{
    public static class RandomFileName
    {
        public static PathInfo GetRandomFileName()
        {
            var fileName = Guid.NewGuid().ToString("N");

            var lowerFileName = new StringBuilder()
                                                        .Append(fileName[0].ToString())
                                                        .Append(fileName[1].ToString())
                                                        .Append(fileName[2].ToString())
                                                        .Append(fileName[3].ToString())
                                                        .ToString()
                                                        .ToLower();
            //todo:use string builder
            var randomDirectory = Path.Combine(lowerFileName);

            return new PathInfo()
            {
                FileName = fileName,
                DirectoryName = randomDirectory
            };
        }
    }
}
