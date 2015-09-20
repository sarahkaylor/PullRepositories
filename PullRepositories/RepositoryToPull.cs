using System;

namespace PullRepositories {
    internal class RepositoryToPull {
        public readonly string Url;
        public readonly string UserName;

        public RepositoryToPull(string userName, string url) {
            UserName = userName;
            Url = url;
        }

        public string HttpUrl {
            get {
                var url = Url;
                if (!(url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
                      url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase)))
                {
                    url = "https://" + url;
                }
                if (!url.EndsWith(".git", StringComparison.CurrentCultureIgnoreCase)) {
                    url += ".git";
                }
                return url;
            }
        }

        public string SshUrl {
            get {
                //git@github.com:dilkaraja94/Homework-1.git
                Uri uri;
                if (Uri.TryCreate(HttpUrl, UriKind.Absolute, out uri)) {
                    var sshUrl = $"git@{uri.Host}:{uri.PathAndQuery}";
                    if (!sshUrl.EndsWith(".git", StringComparison.CurrentCultureIgnoreCase)) {
                        sshUrl += ".git";
                    }
                    return sshUrl;
                }
                return Url;
            }
        }
    }
}