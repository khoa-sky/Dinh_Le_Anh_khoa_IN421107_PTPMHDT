namespace EduHub.Ch02.Logging;

public class ConsoleLogger : IAppLogger
{
    public void Info(string messge) => Console.WriteLine($" [LOG] {messge}");
}