using System.Diagnostics;
using IP_Base;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class PythonAppService : IPythonAppService
    {
        public void StartPythonApp()
        {
            const string scriptPath = @"python-script\main.exe";

            if (!System.IO.File.Exists(scriptPath))
            {
                IP_Message.Error("Unable to find script file main.exe");
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = scriptPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var process = Process.Start(psi);
            process.BeginOutputReadLine();
        }
    }
}