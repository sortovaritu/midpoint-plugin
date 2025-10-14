using System.Collections.Generic;
using IP_Borehole;

namespace WinFormsQtBridge.Plugin.Common.Models
{
    public class WellLogResponse
    {
        public string Name { get; set; }

        public double MinVal { get; set; }

        public double MaxVal { get; set; }

        public double MinMd { get; set; }

        public double MaxMd { get; set; }

        public List<WellLogSample> Log { get; set; }
    }
}