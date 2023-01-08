namespace Endpoint.Core.Models.Syntax.Builders;

public class WebApplicationBuilderModel
{
	public WebApplicationBuilderModel(string dbContextName)
	{
		DbContextName = dbContextName;
	}

    public string DbContextName { get; init; }
}
