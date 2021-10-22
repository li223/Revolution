namespace Revolution.Client.Logging
{
    public interface ILogger
    {
        public void Log(string message, LogLevel logLevel);
    }
}
