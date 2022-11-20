namespace Thabe.Bot;


public static class Log
{
    public static void Write(MessageChain chain)
    {
        StringBuilder sb = new();
        foreach(var i in chain)
        {
            if(i.Type == Messages.Plain)
            {
                sb.Append((i as PlainMessage)!.Text);
            }
            else sb.Append($"{i.Type}");
        }

        Write(sb.ToString());
    }

    public static void Write(Exception e)
    {
        Write(e.Message);
    }

    public static void Write(string msg)
    {

        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}");
    }
}
