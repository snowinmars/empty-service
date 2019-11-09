using System;
using System.Diagnostics;
using System.Linq;
using EmptyService.CommonEntities.Pathes;

namespace EmptyService.CommonEntities.Helpers
{
    // ReSharper disable once AllowPublicClass
    public static class ExecuteHelper
    {
        public static int ExecuteInPowershell(string pureCommand, FilePath log, Action<string> logAction)
        {
            log.Touch();

            var arguments =
                " -NoProfile -ExecutionPolicy ByPass -Command " +
                $"        \"Write-Host $(whoami) ; & {pureCommand} " +
                " | Out-String | Tee-Object '{log}'\" ";

            var info = new ProcessStartInfo("powershell.exe", arguments);
            logAction?.Invoke($"Starting process: 'powershell.exe {arguments}'");

            var process = Process.Start(info);

            if (process is null ||
                process.Id <= 0)
            {
                logAction?.Invoke("Failed to create GUI process from service one");

                throw new Exception("Failed to create GUI process from service one");
            }

            logAction?.Invoke($"powershell started: powershell PID - {process.Id}," +
                              $" command - 'powershell.exe {arguments}'");

            return process.Id;
        }

        public static bool IsProcessAlive(int processId)
        {
            return Process.GetProcesses().Any(x => x.Id == processId);
        }
    }
}