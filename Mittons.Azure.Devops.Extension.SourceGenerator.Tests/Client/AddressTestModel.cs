namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public record AddressTestModel
{
    public int Id { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? City { get; set; }

    public string? StateCode { get; set; }

    public string? CountryCode { get; set; }

    public AddressTestModel() { }

    public AddressTestModel(int id, string line1, string line2, string city, string stateCode, string countryCode)
    {
        Id = id;
        Line1 = line1;
        Line2 = line2;
        City = city;
        StateCode = stateCode;
        CountryCode = countryCode;
    }
}