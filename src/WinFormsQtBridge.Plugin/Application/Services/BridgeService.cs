using Newtonsoft.Json;
using WinFormsQtBridge.Plugin.Application.Services.Interfaces;
using WinFormsQtBridge.Plugin.Common.Models;
using WinFormsQtBridge.Plugin.Common.Models.Enums;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Application.Services
{
    public class BridgeService : IBridgeService
    {
        private readonly IZeroMqServerService _zeroMqServerService;
        
        private readonly IPythonAppService? _pythonAppService;
        
        private readonly IWinFormConnectorService _winFormConnectorService;
        
        public BridgeService(IZeroMqServerService zeroMqServerService, IPythonAppService? pythonAppService, IWinFormConnectorService winFormConnectorService)
        {
            _zeroMqServerService = zeroMqServerService;
            _pythonAppService = pythonAppService;
            _winFormConnectorService = winFormConnectorService;

            _zeroMqServerService.OnActionReceived = OnActionReceived;
        }

        public void Start()
        {
            _zeroMqServerService.Start();
            _pythonAppService?.StartPythonApp();
        }

        private string? OnActionReceived(BridgeActionRequest actionRequest)
        {
            var json = actionRequest.Action switch
            {
                ActionType.GetWellLog => JsonConvert.SerializeObject(_winFormConnectorService.GetSelectedWell()),
                ActionType.SetWellLog => JsonConvert.SerializeObject(_winFormConnectorService.SetWell(actionRequest.Data)),
                _ => null,
            };

            return json;
        }
    }
}