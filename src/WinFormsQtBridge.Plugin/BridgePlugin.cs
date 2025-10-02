using IP_Base;

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
            throw new System.NotImplementedException();
        }
    }
}