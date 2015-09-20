using System;

namespace PullRepositories
{
    class ExceptionHandler
    {
        public void Handle(object sender, UnhandledExceptionEventArgs e) {
            var asSys = e.ExceptionObject as Exception;
            var asApp = e.ExceptionObject as ApplicationException;
            var exitCode = 0;
            if (asApp != null) {
                exitCode = 1;
                Console.Error.WriteLine(asApp.Message);
            } else if (asSys != null) {
                exitCode = 2;
                Console.Error.WriteLine(asSys.Message);
                Console.Error.WriteLine(asSys.StackTrace);
            }
            Environment.Exit(exitCode);
        }
    }
}
