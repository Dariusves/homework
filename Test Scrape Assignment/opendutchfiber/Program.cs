namespace WebScrapingTemplate
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting");

            List<Structs.Address> addresses = Methods.ProcessExcellAddresses(@"..\..\..\Addresses.xlsx");

            List<Structs.AddressProcessed> proccesedAddresses = await Methods.CheckAddressesForFiberAvailability(addresses);

            Methods.WriteResultsToExcel(proccesedAddresses, @"..\..\..\ProcessedAddresses.xlsx");

            Console.WriteLine("Finished");
        }
    }
}