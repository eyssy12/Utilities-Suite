namespace Zagorapps.Core.Library.Extensions
{
    using System.IO;

    public static class PathExtensions
    {
        public static string FindParentDirectory(this string path, string targetName)
        {
            DirectoryInfo info = Directory.GetParent(path);

            if (info == null)
            {
                return string.Empty;
            }

            if (info.Name == targetName)
            {
                return info.FullName;
            }

            return info.FullName.FindParentDirectory(targetName);
        }
    }
}