using System;

namespace Revolution.Client.Logging
{
    public class Logger : ILogger
    {
        private readonly LogLevel _logLevel;

        public Logger(LogLevel logLevel) => _logLevel = logLevel;

        public void Log(string message, LogLevel logLevel)
        {
            if ((int)logLevel < (int)logLevel && logLevel != LogLevel.None)
                return;

            Console.Write("[");
            switch (logLevel)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("DEBUG");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"] - {message} at {DateTime.Now.ToShortTimeString()}\n");
                    break;


                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("ERROR");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"] - {message} at {DateTime.Now.ToShortTimeString()}\n");
                    break;


                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("INFO");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"] - {message} at {DateTime.Now.ToShortTimeString()}\n");
                    break;
            }
        }
    }
}
