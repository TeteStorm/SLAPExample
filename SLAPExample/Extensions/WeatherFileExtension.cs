namespace SLAPExample.Extensions
{
    public static class WeatherFileExtension
    {
        private const string FileExtension = ".json";
        private const int FileLength = 15;
        public static bool IsValidExtension(this string file)
        {
            
            return file.Trim().Substring(file.IndexOf(".", StringComparison.Ordinal), FileExtension.Length) == FileExtension;
        }

        public static bool IsTodayWeatherInfo(this string file)
        {
            var today = DateTime.Today.Date.ToString("dd-MM-yyyy");
            var filename = file.Trim()[..(file.Length - FileExtension.Length)];
            return filename == today;
        }

        public static bool IsTotalSizeLength15(this string file)
        {
            return file.Length == FileLength;
        }
    }
}
