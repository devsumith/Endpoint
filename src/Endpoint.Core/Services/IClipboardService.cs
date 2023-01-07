using System.Threading.Tasks;
using System.Threading;

namespace Endpoint.Core.Services;

public interface IClipboardService
{
    void SetText(string value);

    Task<string> GetTextAsync(CancellationToken cancellationToken = default);
}