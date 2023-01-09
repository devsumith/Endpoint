namespace Endpoint.Core.Models.Syntax.WebApplications;

public class WebApplicationBuilderModel {
    public WebApplicationBuilderModel(string title, string dbContextName)
    {
        Title = title;
        DbContextName = dbContextName;
    }

    public string Title { get; set; }
    public string DbContextName { get; set; }
}
