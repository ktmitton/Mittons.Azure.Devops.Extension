namespace Mittons.Azure.Devops.Extension.Tests.SourceGenerator.Client;

public record NameTestModel
{
    public Guid UniqueIdentifier { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public NameTestModel() { }

    public NameTestModel(Guid uniqueIdentifier, string firstName, string lastName)
    {
        UniqueIdentifier = uniqueIdentifier;
        FirstName = firstName;
        LastName = lastName;
    }
}