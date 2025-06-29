using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;
using System.Net;
using System.Text;

using Server.Router;
using Status = Handlers.Status;

namespace Server;

class Listener
{
    private const int maxConcurrentConnections = 4;
    private static SemaphoreSlim _pool = new SemaphoreSlim(maxConcurrentConnections, maxConcurrentConnections);

    public Listener() { }

    public async Task<HttpListener> InitializeListener()
    {
        Console.WriteLine(" > Initializing Listener\n\n");

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine(" ! Cannot initialize listener on current OS.\n");
            return null;
        }

        HttpListener listener = new HttpListener();

        listener.Prefixes.Add("http://localhost:8080/");
        var localAddresses = GetLocalAddresses();
        foreach (var ip in localAddresses)
        {
            listener.Prefixes.Add("http://" + ip.ToString() + ":8080/");
        }

        listener.Start();

        await RunServer(listener);
        return listener;
    }

    private async Task<Boolean> RunServer(HttpListener listener)
    {
        while (true)
        {
            await StartConnectionListener(listener);
        }
    }

    private async Task StartConnectionListener(HttpListener listener)
    {
        try
        {
            HttpListenerContext _context = await listener.GetContextAsync();
            try
            {
                await _pool.WaitAsync();
                //Router.RouteRequest(_context);
                Status.Response(_context);
            }
            catch (Exception err)
            {
                Console.WriteLine($" ! Error: {err}\n");
            }
            finally
            {
                _pool.Release();
            }
        }
        catch (HttpListenerException err)
        {
            Console.WriteLine($" ! Error: {err}\n");
        }
    }

    private static List<IPAddress> GetLocalAddresses()
    {
        IPHostEntry host;
        host = Dns.GetHostEntry(Dns.GetHostName());
        List<IPAddress> localAddresses = host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        Console.WriteLine(" > Local Addresses:");
        foreach (var ip in localAddresses)
        {
            Console.WriteLine($" [ {ip} ]");
        }
        Console.WriteLine($"\n");
        return localAddresses;
    }

    private static async Task<List<IPAddress>> ResolveAddressAsync()
    {
        var ext = await Dns.GetHostAddressesAsync("google.com");
        var externalAddresses = ext.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToList();
        Console.WriteLine("\nRequested Addresses: ");
        foreach (var ip in externalAddresses)
        {
            Console.WriteLine($" : {ip}");
        }
        return externalAddresses;
    }
}