using System.IO;

namespace Router;

public class Router
{
    public static byte[] Get()
    {
        byte[] document = System.IO.File.ReadAllBytes("./Content/Pages/index.html");
        return document;
    }
}