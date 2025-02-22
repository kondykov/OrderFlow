namespace OrderFlow.Data.Interfaces;

public interface IDataSeeder
{
    public Task SeedAsync(IServiceProvider serviceProvider);
}