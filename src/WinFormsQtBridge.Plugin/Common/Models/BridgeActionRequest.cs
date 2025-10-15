using WinFormsQtBridge.Plugin.Common.Models.Enums;

namespace WinFormsQtBridge.Plugin.Common.Models
{
    public class BridgeActionRequest
    {
        public ActionType Action { get; set; }
        
        public string? Data { get; set; }
    }
}