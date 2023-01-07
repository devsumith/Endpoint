using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.Services;

public class ClipboardService: IClipboardService
{
    public void SetText(string value)
    {
        TextCopy.ClipboardService.SetText(value);
    }

    public async Task<string> GetTextAsync(CancellationToken cancellationToken = default)
    {
        return await TextCopy.ClipboardService.GetTextAsync(cancellationToken);
    }
}
