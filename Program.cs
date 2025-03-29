namespace BasicHTTP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool ssl = Utils.RequestYN("SSL/TLS", true);

            Uri? site = Utils.BuildDomain(ssl);
            if (site is null) { Console.WriteLine("Invalid URL"); return; }

            RequestHTTP(site).Wait();
            Console.ReadLine();
        }

        private static async Task RequestHTTP(Uri site)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            HttpClient client = new();
            try
            {
                using HttpResponseMessage? response = await client.GetAsync(site);

                if (response is null) { Console.WriteLine("Response was null."); return; }
                await Utils.PrintResponse(response, site);
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch (Exception ex) 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Error.WriteLine("An error occured while requesting the content.");
                Console.Error.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.Red;
            }
            finally 
            { 
                client.Dispose(); 
                Console.WriteLine("\nRequest finished.");
                Console.ForegroundColor = defaultColor;
            }
        }
    }
}
