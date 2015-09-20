using System;
using System.Diagnostics;

namespace PullRepositories {
    internal class GitLauncher {
        private readonly string _url;
        private readonly TemporaryFolder _tempFolder;
        public int ExitCode;

        public GitLauncher(string url, TemporaryFolder tempFolder) {
            _url = url;
            _tempFolder = tempFolder;
            ExitCode = 0;
        }

        public void RunCloneToCompletion() {
            var startInfo = new ProcessStartInfo {
                FileName = AppSettings.GitPath,
                Arguments = $"clone {_url}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = _tempFolder.Path.FullName
            };
            using (var proc = Process.Start(startInfo)) {
                if (proc == null) {
                    throw new Exception("failed to start " + AppSettings.GitPath);
                }
                using (new StreamDuplicator(proc.StandardOutput, Console.Out, proc)) {
                    using (new StreamDuplicator(proc.StandardError, Console.Out, proc)) {
                        while (!proc.HasExited) {
                            proc.Refresh();
                        }
                        ExitCode = proc.ExitCode;
                    }
                }
            }
        }
    }
}