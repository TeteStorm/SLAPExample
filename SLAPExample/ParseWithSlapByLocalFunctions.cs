using Newtonsoft.Json;

using SLAPExample.Extensions;
using SLAPExample.Models;

namespace SLAPExample
{
    public static class ParseWithSlapByLocalFunctions
    {
        public static List<WeatherDataDto> GetWeatherReport(string directoryName)
        {
            var weatherDtoList = new List<WeatherDataDto>();

            var file = GetWeatherReportFile(directoryName);

            if (file is not { Exists: true } || !IsValidFile(file.Name)) return weatherDtoList;
            {
                var weatherReport = ReadWeatherReport(file.FullName);

                if (WeatherReportHasData(weatherReport))
                {
                    MapToReportDto(weatherDtoList, weatherReport);
                }
            }

            return weatherDtoList;

            static FileInfo? GetWeatherReportFile(string directoryName)
            {
                var di = new DirectoryInfo(directoryName);
                return di.Exists && di.GetFiles("*.json").Any() ? di.GetFiles().FirstOrDefault() : null;
            }


            static bool IsValidFile(string file)
            {
                return file.IsValidExtension() && file.IsTodayWeatherInfo() && file.IsTotalSizeLength15();
            }


            static WeatherDataModel[] ReadWeatherReport(string file)
            {
                return JsonConvert.DeserializeObject<WeatherDataModel[]>(File.ReadAllText(file)) ?? Array.Empty<WeatherDataModel>();
            }

            static bool WeatherReportHasData(WeatherDataModel[] weatherReport)
            {
                return weatherReport is { Length: > 0 };
            }


            static void MapToReportDto(List<WeatherDataDto> weatherDtoList, IEnumerable<WeatherDataModel> weatherReport)
            {
                weatherDtoList.AddRange(weatherReport.Select(report => new WeatherDataDto()
                {
                    City = report.city,
                    Temperature = report.temp,
                    ReportedBy = report.reportedBy
                }));
            }
        }
    }
}