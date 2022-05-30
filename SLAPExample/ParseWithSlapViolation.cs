using Newtonsoft.Json;

using SLAPExample.Models;

namespace SLAPExample
{
    public static class ParseWithSlapViolation
    {
        public static List<WeatherDataDto> GetWeatherReport()
        {
            DirectoryInfo di = new DirectoryInfo("Data"); //Abstr 1 - Aqui temos a busca do arquivo de temperaturas do dia no diretório de entrada
            var file = di.GetFiles().FirstOrDefault();

            var weatherDtoList = new List<WeatherDataDto>();

            if (ValidateFile(file.Name)) //Abstr 2 - Aqui temos a validação do arquivo de entrada
            {
                // Abstr 3 - Neste nível, o JSON do relatório de temperatura é parseado com a ajuda do JSON parser(no caso a biblioteca NewtonSoft)
                var weatherReport = JsonConvert.DeserializeObject<WeatherDataModel[]>(File.ReadAllText(file.FullName));

                if (weatherReport != null && weatherReport.Length > 0) //Abstr 4 - Aqui acontece a validação do relatório de temperatura já serializado
                {
                    foreach (var report in weatherReport)
                    {
                        var weatherDto = new WeatherDataDto() //Abstr 5 - Neste nível os dados do DTO recebido são mapeados para no nosso modelo de leitura
                        {
                            City = report.city,
                            Temperature = report.temp,
                            ReportedBy = report.reportedBy
                        };
                        weatherDtoList.Add(weatherDto);
                    }
                }
            }

            return weatherDtoList;
        }

        private static bool ValidateFile(string file)
        {
            // Nesta validação temos uma mistura de código que representa o que precisa ser validado com as próprias políticas de validação em si

            const string extension = ".json";
            const int size = 15;

            var filename = file.Trim().Substring(0, file.Length - extension.Length);
            var fileExtension = file.Trim().Substring(file.IndexOf(".", StringComparison.Ordinal), extension.Length);
            var today = DateTime.Today.Date.ToString("dd-MM-yyyy");

            //what made a valid policy
            bool hasTodayWeatherInfo = filename == today;
            bool hasJsonExtension = fileExtension == extension;
            bool totalSizeLengthIs18 = file.Trim().Length == size;


            return hasTodayWeatherInfo && hasJsonExtension && totalSizeLengthIs18;
        }
    }
}
