using IP_Base;
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
        
        public void Execute(ProjectTreeView project)
        {
            var pythonAppService = new PythonAppService();
            var pipeServerService = new PipeServerService();
            var winFormConnectorService = new WinFormConnectorService();
            var bridgeService = new BridgeService(pipeServerService, pythonAppService, winFormConnectorService);
        }
    }
}