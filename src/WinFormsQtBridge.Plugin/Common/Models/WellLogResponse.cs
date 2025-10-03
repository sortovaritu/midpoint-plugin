using System.Collections.Generic;
using IP_Borehole;

namespace WinFormsQtBridge.Plugin.Common.Models
{
    public class WellLogResponse
    {
        public string Name { get; set; }
        
        public List<WellLogSample> WellLogSamples { get; set; }
    }
}