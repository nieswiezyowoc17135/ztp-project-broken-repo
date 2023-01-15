public sealed class Logger
{
    private static Logger instance;

    private Logger() { }

    public static Logger GetInstance()
    {
        if (instance == null)
        {
            instance = new Logger();
        }
        return instance;
    }

    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}