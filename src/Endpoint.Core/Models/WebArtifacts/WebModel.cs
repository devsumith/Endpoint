using System.Collections.Generic;

namespace Endpoint.Core.Models.WebArtifacts;

public class WebModel
{
	public WebModel()
	{
		NpmPackages = new List<NpmPackageModel>();
	}
    public List<NpmPackageModel> NpmPackages { get; set; }
}
