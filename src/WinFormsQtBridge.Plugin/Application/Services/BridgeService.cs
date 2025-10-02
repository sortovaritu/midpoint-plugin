using WinFormsQtBridge.Plugin.Application.Services.Interfaces;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Application.Services
{
    public class BridgeService : IBridgeService
    {
        private readonly IPipeServerService _pipeServerService;
        
        private readonly IPythonAppService _pythonAppService;
        
        private readonly IWinFormConnectorService _winFormConnectorService;
        
        public BridgeService(IPipeServerService pipeServerService, IPythonAppService pythonAppService, IWinFormConnectorService winFormConnectorService)
        {
            _pipeServerService = pipeServerService;
            _pythonAppService = pythonAppService;
            _winFormConnectorService = winFormConnectorService;

            _pipeServerService.OnActionReceived = OnActionReceived;
        }

        private string OnActionReceived(string action)
        {
            return string.Empty;
        }
    }
}