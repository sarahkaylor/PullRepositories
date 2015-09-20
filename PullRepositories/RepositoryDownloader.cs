using System;
using System.IO;
using System.Linq;

namespace PullRepositories
{
    internal class RepositoryDownloader
    {
        internal void CloneRepository(RepositoryToPull entry) {
            var gitUrl = MakeGitPullUrl(entry.Url);
            var pwd = new DirectoryInfo(".");
            using (var tempFolder = new TemporaryFolder()) {
                var git = new GitLauncher(gitUrl, tempFolder);
                git.RunCloneToCompletion();
                MoveResult(entry.UserName, tempFolder, pwd);
            }
        }

        private static void MoveResult(string unformattedUserName, TemporaryFolder tempFolder, DirectoryInfo pwd) {
            var childFolder = tempFolder.Path.GetDirectories().First();
            var userName = unformattedUserName.Replace(" ", "_").Replace(":", " ").Replace("-", "_");
            var destPath = Path.Combine(pwd.FullName, userName);
            Directory.Move(childFolder.FullName, destPath);
        }

        private static string MakeGitPullUrl(string uri) {
            if (!uri.EndsWith(".git", StringComparison.CurrentCultureIgnoreCase)) {
                return uri + ".git";
            }
            return uri;
        }
    }
}