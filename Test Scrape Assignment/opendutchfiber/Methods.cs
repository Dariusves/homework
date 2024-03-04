using ClosedXML.Excel;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using Newtonsoft.Json.Linq;

namespace WebScrapingTemplate
{
    public class Methods
    {
        public static async Task<List<Structs.AddressProcessed>> CheckAddressesForFiberAvailability(List<Structs.Address> adresses)
        {
            List<Structs.AddressProcessed> processedAddresses = [];
            try
            {
                using var client = new HttpClient();
                foreach (Structs.Address address in adresses)
                {
                    string hasFiber = string.Empty;
                    string fiberStatus = string.Empty;

                    List<string> postBody = [
                        "zipcode=" + address.PostalCode,
                            "house_number=" + address.HouseNumber,
                            "house_number_suffix=" + address.HouseNumberExtension,
                            "version=2",
                            "inline=true"
                    ];

                    HttpRequestMessage postRequest = CreatePostRequest(new Uri(HeadersUrls.BaseUrl + HeadersUrls.PostUrl), HeadersUrls.PostHeaders, postBody);
                    HttpResponseMessage postResponse = await CreateResponse(postRequest, client);

                    string body = GetBody(postResponse);

                    JObject addressInformation = JObject.Parse(body);
                    string addressFiberStatusJSON = addressInformation.SelectToken("#search-location-alert").ToString();
                    fiberStatus = Regex.Match(addressFiberStatusJSON, @"\u0022alert\u0022>(.*)\.", RegexOptions.Singleline).Groups[1].Value.Trim();

                    /* 
                    example response
                    
                    <div class="" id="search-location-alert">
                    <div class="alert alert-success mt-4" role="alert">
                    Gefeliciteerd! In jouw straat ligt al glasvezel. Wil je een aansluiting? Sluit dan een abonnement af bij een glasvezel provider.
                    </div>
                    </div>
                    */

                    if (Regex.Match(fiberStatus, "Gefeliciteerd").Success)
                    {
                        hasFiber = "Yes";
                    }
                    else
                    {
                        hasFiber = "No";
                        fiberStatus = fiberStatus.Replace("\r\n", " ");
                    }

                    processedAddresses.Add(new()
                    {
                        Address = address,
                        HasFiber = hasFiber,
                        FiberStatus = fiberStatus,
                    });
                }

                return processedAddresses;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured:\r\n{ex.Message}");
                Console.WriteLine($"Got blocked or need to adjust Regexe's");
                return processedAddresses;
            }
        }

        public static List<Structs.Address> ProcessExcellAddresses(string filePath)
        {
            char hangulFiller = '\u3164';

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fileInfo = new(filePath);

            List<Structs.Address> adresses = [];

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Structs.Address tempAddress = new()
                    {
                        LocationName = worksheet.Cells[row, 1].Value?.ToString().Replace(hangulFiller.ToString(), ""),
                        PostalCode = worksheet.Cells[row, 2].Value?.ToString().Replace(hangulFiller.ToString(), ""),
                        HouseNumber = worksheet.Cells[row, 3].Value?.ToString().Replace(hangulFiller.ToString(), ""),
                        HouseNumberExtension = worksheet.Cells[row, 4].Value?.ToString().Replace(hangulFiller.ToString(), "")
                    };
                    if (!tempAddress.Equals(default(Structs.Address)))
                    {
                        adresses.Add(tempAddress);
                    }
                }
            }

            return adresses;
        }

        public static HttpRequestMessage CreatePostRequest(Uri uri, NameValueCollection headers, List<string> postBody)
        {
            StringContent content = new(string.Join('&', postBody), Encoding.ASCII, "application/x-www-form-urlencoded");

            HttpRequestMessage request = new(HttpMethod.Post, uri)
            {
                Content = content
            };

            headers.Add("Content-Length", $"{content.ToString().Length}");

            foreach (string key in headers.AllKeys)
            {
                request.Headers.TryAddWithoutValidation(key, headers[key]);
            }

            return request;
        }
        public static async Task<HttpResponseMessage> CreateResponse(HttpRequestMessage request, HttpClient client)
        {
            HttpResponseMessage response;
            Console.Write(request.RequestUri + "  ");

            response = await client.SendAsync(request);

            Console.WriteLine(response.StatusCode);
            return response;
        }

        public static string GetBody(HttpResponseMessage response)
        {
            StreamReader reader = new(response.Content.ReadAsStream());

            return reader.ReadToEnd();
        }

        public static void WriteResultsToExcel(List<Structs.AddressProcessed> proccesedAddresses, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Addresses");

                worksheet.Cell("A1").Value = "Location Name";
                worksheet.Cell("B1").Value = "Postal Code";
                worksheet.Cell("C1").Value = "House Number";
                worksheet.Cell("D1").Value = "House Number Extension";
                worksheet.Cell("E1").Value = "Has Fiber";
                worksheet.Cell("F1").Value = "Fiber Status";

                int currentRow = 2;
                foreach (Structs.AddressProcessed proccessedAddress in proccesedAddresses)
                {
                    worksheet.Cell(currentRow, 1).Value = proccessedAddress.Address.LocationName;
                    worksheet.Cell(currentRow, 2).Value = proccessedAddress.Address.PostalCode;
                    worksheet.Cell(currentRow, 3).Value = proccessedAddress.Address.HouseNumber;
                    worksheet.Cell(currentRow, 4).Value = proccessedAddress.Address.HouseNumberExtension;
                    worksheet.Cell(currentRow, 5).Value = proccessedAddress.HasFiber;
                    worksheet.Cell(currentRow, 6).Value = proccessedAddress.FiberStatus;
                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
        }
    }
}