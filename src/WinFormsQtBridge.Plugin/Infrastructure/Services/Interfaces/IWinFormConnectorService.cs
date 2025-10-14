using WinFormsQtBridge.Plugin.Common.Models;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces
{
    public interface IWinFormConnectorService
    {
        WellLogResponse GetSelectedWell();

        WellLogResponse SetWell(string data);
    }
}