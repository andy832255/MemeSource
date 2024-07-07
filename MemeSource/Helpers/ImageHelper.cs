namespace MemeSource.Helpers
{
    public static class ImageHelper
    {
        public static string GenerateFileName(string username, string url)
        {
            return $"{username}_{Path.GetFileName(url)}";
        }
    }
}