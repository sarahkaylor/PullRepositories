using System.Collections.Generic;
using System.Configuration;

namespace PullRepositories {
    internal class AppSettings {
        public static string GitPath => Get("GitPath");

        public static string DestPath => Get("DestPath");

        private static string Get(string key) {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value)) {
                throw new KeyNotFoundException(key);
            }
            return value;
        }
    }
}