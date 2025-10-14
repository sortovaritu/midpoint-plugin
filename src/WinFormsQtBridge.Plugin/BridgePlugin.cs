using System;
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

        private Logger _logger = LogManager.GetCurrentClassLogger();
        
        private IDisposable _webApp;
        
        public void Execute(ProjectTreeView project)
        {
            _logger.Info("Loading plugin...");
            var pythonAppService = new PythonAppService();
            var winFormConnectorService = new WinFormConnectorService(project);
            var zeroMqServerService = new ZeroMqServerService(project);
            var bridgeService = new BridgeService(zeroMqServerService, pythonAppService, winFormConnectorService);
            
            bridgeService.Start();
        }
    }
}