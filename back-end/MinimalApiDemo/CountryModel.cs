using System;
using Bogus;

namespace MinimalApiDemo;

public record class CountryModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string FlagUri { get; init; }
    public string CapitalCity { get; init; }
    public string Anthem { get; init; }
    public IEnumerable<string> Languages { get; init; }
}
public class CountryModelFaker : Faker<CountryModel>
{
    public CountryModelFaker()
    {
        // Use seed 42
        Randomizer.Seed = new Random(42);

        // Faker rules
        RuleFor(o => o.Id, f => f.UniqueIndex);
        RuleFor(o => o.Name, f => f.Address.Country());
        RuleFor(o => o.Description, f => f.Lorem.Paragraph());
        RuleFor(o => o.FlagUri, f => f.Internet.Url());
        RuleFor(o => o.CapitalCity, f => f.Address.City());
        RuleFor(o => o.Anthem, f => f.Lorem.Sentence());
        RuleFor(o => o.Languages, f => f.Lorem.Words(3));
    }
}