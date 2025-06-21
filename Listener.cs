global using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using System.Text;

class Listener
{
    private const int maxConcurrentConnections = 4;
    private static Semaphore _pool = new Semaphore(maxConcurrentConnections, maxConcurrentConnections);
    static async Task Main()
    {
        HttpListener listener = InitializeListener(GetLocalAddresses());
        listener.Start();
        await Task.Run(() => RunServer(listener));
    }

    static HttpListener InitializeListener(List<IPAddress> localAddresses)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");

        foreach (var ip in localAddresses)
        {
            Console.WriteLine("http://" + ip.ToString() + ":8080/");
            listener.Prefixes.Add("http://" + ip.ToString() + ":8080/");
        }

        return listener;
    }

    private async static Task<Boolean> RunServer(HttpListener listener)
    {
        while (true)
        {
            _pool.WaitOne();
            await StartConnectionListener(listener);
        } 
        return true;
    }

    private async static Task StartConnectionListener(HttpListener listener)
    {
        try
        {
            HttpListenerContext _context = await listener.GetContextAsync();
            
            string response = "Hello Browser!";
            byte[] encoded = Encoding.UTF8.GetBytes(response);
            _context.Response.ContentLength64 = encoded.Length;
            _context.Response.OutputStream.Write(encoded, 0, encoded.Length);
            _context.Response.OutputStream.Close();

            return;
        }
        catch (HttpListenerException err)
        {
            _pool.Release();
            Console.WriteLine($"Error {err}");
        }
    }

    public static List<IPAddress> GetLocalAddresses()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        List<IPAddress> localAddresses = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        Console.WriteLine("\nLocal Addresses: ");
        foreach (var ip in localAddresses)
        {
            Console.WriteLine($"> {ip}");
        }
        return localAddresses;
    }

    public static async Task<List<IPAddress>> ResolveAddressAsync()
    {
        var ext = await Dns.GetHostAddressesAsync("google.com");
        var externalAddresses = ext.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        Console.WriteLine("\nRequested Addresses: ");
        foreach (var ip in externalAddresses)
        {
            Console.WriteLine($"> {ip}");
        }
        return externalAddresses;
    }
}