using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PullRepositories {
    internal class EntryReader {
        public IEnumerable<RepositoryToPull> Read(FileInfo file) {
            var repositoriesToPull =
                from line in ReadLines(file)
                let repo = ParseLine(line)
                where IsValid(repo)
                select repo;
            return repositoriesToPull.ToList();
        }

        private RepositoryToPull ParseLine(string line) {
            var parts = line.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var name = parts[1];
            var url = parts[2];
            return new RepositoryToPull(name, url);
        }

        private bool IsValid(RepositoryToPull repo) {
            if (string.IsNullOrWhiteSpace(repo.UserName)) {
                return false;
            }
            Uri uri;
            if (!Uri.TryCreate(repo.Url, UriKind.Absolute, out uri)) {
                return false;
            }
            if (string.Compare("github.com", uri.Host, StringComparison.CurrentCultureIgnoreCase) != 0) {
                return false;
            }
            var pathParts = uri.AbsolutePath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            if (pathParts.Length != 2) {
                return false;
            }
            return true;
        }

        private IEnumerable<string> ReadLines(FileInfo file) {
            var first = true;
            using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read)) {
                using (var reader = new StreamReader(stream)) {
                    string line;
                    do {
                        line = reader.ReadLine();
                        if (first) {
                            first = false;
                        } else {
                            if (!string.IsNullOrWhiteSpace(line)) {
                                yield return line;
                            }
                        }
                    } while (line != null);
                }
            }
        }
    }
}