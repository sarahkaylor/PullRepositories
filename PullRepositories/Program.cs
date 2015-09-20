using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PullRepositories {
    internal class Program {
        private static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += new ExceptionHandler().Handle;
            //var file = new FileInfo(args[0]);
            var file = new FileInfo(@"C:\Users\Sarah\Downloads\responses_hw1.csv");
            var entries = new EntryReader().Read(file);
            var downloader = new RepositoryDownloader();
            foreach (var entry in entries) {
                downloader.CloneRepository(entry);
            }
        }
    }
}