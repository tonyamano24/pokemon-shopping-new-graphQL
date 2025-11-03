using System;

namespace itsc_dotnet_practice.Models;

public class Log
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string LogLevel { get; set; } = "Information"; // Default log level
    public string? Exception { get; set; } // Optional exception details
    public Log() { }
    public Log(string message, string logLevel = "Information", string? exception = null)
    {
        Message = message;
        LogLevel = logLevel;
        Exception = exception;
    }
}
