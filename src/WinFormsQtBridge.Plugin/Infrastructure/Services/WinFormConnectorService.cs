using System;
using System.Collections.Generic;
using IP_Base;
using IP_Borehole;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;
using WinFormsQtBridge.Plugin.Common.Models;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class WinFormConnectorService : IWinFormConnectorService
    {
        private readonly ProjectTreeView _project;
        
        private readonly List<WellLog> _allWellLog;

        public WinFormConnectorService(ProjectTreeView project)
        {
            _project = project;
        }
        
        public WellLogResponse GetSelectedWell()
        {
            if (_project.SelectedNode == null)
            {
                return null;
            }
	        
            try
            {
                var projectNode = _project.SelectedNode;
                var wellNode = projectNode.Tag as WellLog ?? throw new ArgumentNullException(nameof(projectNode.Tag));
                
                return new WellLogResponse()
                {
                    Name = wellNode.Name,
                    WellLogSamples = wellNode.Log,
                };
            }
            catch (Exception ex)
            {
                IP_Message.Error(ex.Message + ex.StackTrace + ex.Source);
            }

            return null;
        }
    }
}