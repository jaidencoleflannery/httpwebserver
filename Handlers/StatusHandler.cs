using System.Net;
using System.Text;
using System.Threading;

namespace Handlers;
class Status
{
    public async static void Response(HttpListenerContext _context)
    {
        string response = "<html><body>Status: Running!</html></body>";
        byte[] encoded = Encoding.UTF8.GetBytes(response);
        _context.Response.ContentLength64 = encoded.Length;
        _context.Response.OutputStream.Write(encoded, 0, response.Length);
        _context.Response.OutputStream.Close();
        Console.WriteLine($" > Request [{DateTime.Now:T}] {_context.Request.HttpMethod} {_context.Request.Url}");
    }
}