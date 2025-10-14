using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IP_Base;
using IP_Borehole;
using Newtonsoft.Json;
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
                var projectNode = _project.SelectedNode.Tag as ProjectNode ?? throw new ArgumentNullException(nameof(_project.SelectedNode.Tag));
                var wellNode = projectNode.ProjectObject as WellLog ?? throw new ArgumentNullException(nameof(projectNode.ProjectObject));

                var well = new WellLogResponse
                {
                    Name = wellNode.Name,
                    MinVal = wellNode.MinVal,
                    MaxVal = wellNode.MaxVal,
                    MinMd = wellNode.MinMd,
                    MaxMd = wellNode.MaxMd,
                    Log = wellNode.Log,
                };

                return well;
            }
            catch (Exception ex)
            {
                IP_Message.Error(ex.Message + ex.StackTrace + ex.Source);
            }

            return null;
        }

        public WellLogResponse SetWell(string data)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<WellLogResponse>(data);

                var treeNode = _project.SelectedNode.Parent;
                
                var projectNode = treeNode.Tag as ProjectNode ?? throw new ArgumentNullException(nameof(_project.SelectedNode.Parent.Tag));

                var borehole = projectNode.ProjectObject as Borehole ?? throw new ArgumentNullException(nameof(projectNode.ProjectObject));

                var wall = new WellLog()
                {
                    Name = model.Name,
                    MinVal = model.MinVal,
                    MaxVal = model.MaxVal,
                    MinMd = model.MinMd,
                    MaxMd = model.MaxMd,
                    Log = model.Log,
                };
                
                borehole.Logs.Add(wall);
                
                var wellLogNode = _project.CreateProjectInputNode(wall.Name, new ProjectNode(
                    "WellsLog",
                    ProjectNode.ProjectUnits.WellLog,
                    wall,
                    wall.MinVal,
                    wall.MaxVal));
                
                treeNode.Nodes.Add(wellLogNode);

                IP_IO.CurrectProject.SetChanged();
            }
            catch (Exception ex)
            {
                IP_Message.Error(ex.Message + ex.StackTrace + ex.Source);
            }
            
            return null;
        }
    }
}