using System;
using System.IO;

namespace PullRepositories {
    internal class TemporaryFolder : IDisposable {
        public readonly DirectoryInfo Path;

        public TemporaryFolder() {
            var tempFolder = System.IO.Path.GetTempPath();
            var id = Guid.NewGuid().ToString();
            var path = System.IO.Path.Combine(tempFolder, id);
            Directory.CreateDirectory(path);
            Path = new DirectoryInfo(path);
        }

        public void Dispose() {
            Directory.Delete(Path.FullName, true);
        }
    }
}