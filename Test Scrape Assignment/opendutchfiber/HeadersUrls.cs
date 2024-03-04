namespace WebScrapingTemplate
{
    using System.Collections.Specialized;

    public class HeadersUrls
    {
        public static string BaseUrl = "https://opendutchfiber.nl/";
        public static string PostUrl = BaseUrl + "resultaat";
        public static NameValueCollection PostHeaders = new()
                {
                    {"Host","opendutchfiber.nl"},
                    {"Connection","keep-alive"},
                    {"sec-ch-ua","\"Not A(Brand\";v=\"99\", \"Microsoft Edge\";v=\"121\", \"Chromium\";v=\"121\""},
                    {"X-OCTOBER-REQUEST-PARTIALS",""},
                    {"sec-ch-ua-mobile","?0"},
                    {"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 Edg/121.0.0.0"},
                    {"X-OCTOBER-REQUEST-HANDLER","postcodeCheck::onCheckZipcode"},
                    {"Accept","*/*"},
                    {"X-Requested-With","XMLHttpRequest"},
                    {"X-OCTOBER-REQUEST-FLASH","1"},
                    {"sec-ch-ua-platform","\"Windows\""},
                    {"Sec-Fetch-Site","same-origin"},
                    {"Origin",BaseUrl},
                    {"Sec-Fetch-Mode","cors"},
                    {"Sec-Fetch-Dest","empty"},
                    {"Referer",PostUrl},
                    {"Accept-Encoding","br"},
                    {"Accept-Language","en-US,en;q=0.9"},
                    {"Content-Type","application/x-www-form-urlencoded; charset=us-ascii"},
                };
    }
}