public class MyCustomLogger : ISuperLogger
{
    public void Log(string toLog)
    {
        UnityEngine.Debug.Log(toLog);
    }

    ~MyCustomLogger()
    {
        UnityEngine.Debug.Log("MyCustomLogger was destroyed from the GC");
    }
}