using DfnvUtilities;
using System.Text;

namespace BasicHTTP
{
    class Utils
    {
        public static bool RequestYN(string question, bool defaultResponse = false)
        {
            Console.Write(question + " (y/n) -> ");
            string response = Console.ReadLine().ToLower();
            if (string.IsNullOrEmpty(response)) return defaultResponse;
            else return response.Contains('y');
        }

        public static Uri? BuildDomain( bool ssl)
        {
            Console.Write("Enter Domain -> ");
            string domain = Console.ReadLine();
            if (domain is null) return null;

            StringBuilder sb = new(domain);

            if (domain.StartsWith("http://")) sb.Remove(0, 7);
            else if (domain.StartsWith("https://")) sb.Remove(0, 8);

            sb.Insert(0, ssl ? "https://" : "http://");

            return new Uri(sb.ToString());
        }

        public static async Task PrintResponse(HttpResponseMessage response, Uri uri)
        {
            DfnvUtilities.Printer printer = new() { Chunksize = 200 };
            printer.Add("HTTP-Request Report on");
            printer.Add($"{uri.Host} (Port {uri.Port})");
            printer.AddSeperator(Printer.LineSeperator);
            printer.Add("Sucess: " + response.IsSuccessStatusCode);
            printer.Add("Status Code: " + response.StatusCode);
            printer.Add("Version: " + response.Version);
            printer.AddSeperator(Printer.BlockSeparator);
            printer.Add("Headers ");
            printer.AddSeperator(Printer.LineSeperator);
            foreach (var item in response.Headers)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var value in item.Value)
                {
                    stringBuilder.Append(value + " ");
                }
                printer.Add(item.Key + " : " + stringBuilder.ToString());
            }


            if (response.IsSuccessStatusCode && RequestYN("Print Content"))
            {
                printer.AddSeperator(Printer.BlockSeparator);
                printer.Add("Content: ");
                printer.AddSeperator(Printer.LineSeperator);

                printer.Add(await response.Content.ReadAsStringAsync());
            }
            response.Dispose();
            printer.Print();
        }
    }
}
