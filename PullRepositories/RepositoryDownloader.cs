using System;
using System.IO;
using System.Linq;

namespace PullRepositories
{
    internal class RepositoryDownloader
    {
        internal void CloneRepository(RepositoryToPull entry) {
            var pwd = new DirectoryInfo(AppSettings.DestPath);
            if (!pwd.Exists) {
                pwd.Create();
            }
            using (var tempFolder = new TemporaryFolder()) {
                var git = new GitLauncher(entry.HttpUrl, tempFolder);
                git.RunCloneToCompletion();
                if (git.ExitCode != 0) {
                    Console.WriteLine("Failed to download {0} on HTTP, trying SSH", entry.HttpUrl);
                    git = new GitLauncher(entry.SshUrl, tempFolder);
                    git.RunCloneToCompletion();
                }
                if (git.ExitCode == 0) {
                    Console.WriteLine("Succeeded downloading");
                    Console.WriteLine("Moving to {0}", pwd);
                }
                MoveResult(entry.UserName, tempFolder, pwd);
            }
        }

        private static void MoveResult(string unformattedUserName, TemporaryFolder tempFolder, DirectoryInfo pwd) {
            var childFolder = tempFolder.Path.GetDirectories().First();
            var userName = unformattedUserName.Replace(" ", "_").Replace(":", " ").Replace("-", "_");
            var destPath = Path.Combine(pwd.FullName, userName);
            Directory.Move(childFolder.FullName, destPath);
        }
    }
}