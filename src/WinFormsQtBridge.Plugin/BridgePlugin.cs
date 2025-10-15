using IP_Base;
using NLog;
using WinFormsQtBridge.Plugin.Application.Services;
using WinFormsQtBridge.Plugin.Infrastructure.Services;

namespace WinFormsQtBridge.Plugin
{
    public class BridgePlugin : IPlugin
    {
        public string Name => "BridgePlugin";
        public string Description => "Bridge plugin";
        public string Author => "Dmitry Danko";
        public string Version => "0.1";

        private PythonAppService? _pythonAppService;

        private WinFormConnectorService? _winFormConnectorService;

        private ZeroMqServerService? _zeroMqServerService;

        private BridgeService? _bridgeService;

        private Logger _logger = LogManager.GetCurrentClassLogger();
        
        public void Execute(ProjectTreeView project)
        {
            _pythonAppService = new PythonAppService();
            _winFormConnectorService = new WinFormConnectorService(project);
            _zeroMqServerService = new ZeroMqServerService(project);
            _bridgeService = new BridgeService(_zeroMqServerService, _pythonAppService, _winFormConnectorService);
            
            _bridgeService.Start();
        }
    }
}