using System.Diagnostics;
using IP_Base;
using WinFormsQtBridge.Plugin.Infrastructure.Services.Interfaces;

namespace WinFormsQtBridge.Plugin.Infrastructure.Services
{
    public class PythonAppService : IPythonAppService
    {
        private void StartPythonApp()
        {
            const string pythonExe = @"python-script\venv\Scripts\python.exe";
            const string scriptPath = @"python-script\main.py";
            
            if (!System.IO.File.Exists(pythonExe))
            {
                IP_Message.Error("Unable to find python.exe file");
                return;
            }

            if (!System.IO.File.Exists(scriptPath))
            {
                IP_Message.Error("Unable to find script file main.py");
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{scriptPath}\"",
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