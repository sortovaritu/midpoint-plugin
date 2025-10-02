using IP_Base;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class WinFormConnectorService : IWinFormConnectorService
    {
        private readonly ProjectTreeView _project;

        public WinFormConnectorService(ProjectTreeView project)
        {
            _project = project;
        }
    }
}