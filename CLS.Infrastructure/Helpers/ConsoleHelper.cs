using System;

namespace CLS.Infrastructure.Helpers
{
    public static class ConsoleHelper
    {
        public static void LogMessageToConsole(string message = null, Exception exception = null)
        {
            if (message != null)
            {
                Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]" + message);
            }
            if (exception != null)
            {
                Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}]" + exception.GetExceptionMessages());
            }
        }

        public static void LogColouredMessageToConsole(ConsoleColor colour, string message = null, Exception exception = null)
        {
            Console.ForegroundColor = colour;
            LogMessageToConsole(message, exception);
            Console.ResetColor();
        }
    }
}
