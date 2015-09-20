using System;

namespace PullRepositories {
    internal class RepositoryToPull {
        public readonly string Url;
        public readonly string UserName;

        public RepositoryToPull(string userName, string url) {
            if (!(url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
                  url.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))) {
                url = "https://" + url;
            }
            UserName = userName;
            Url = url;
        }
    }
}