namespace WebScrapingTemplate
{
    public class Structs
    {
        public struct Address
        {
            public string? LocationName;
            public string? PostalCode;
            public string? HouseNumber;
            public string? HouseNumberExtension;
        }

        public struct AddressProcessed
        {
            public Address Address;
            public string HasFiber;
            public string FiberStatus;
        }
    }
}