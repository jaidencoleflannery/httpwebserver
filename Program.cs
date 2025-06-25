global using System;
using System.Net;
using System.Threading.Tasks;
using Listener = Server.Listener;
using Handlers;

Console.WriteLine("\n\n [ Starting Local HTTP Webserver ] \n\n");

Listener _listener = new Listener();
Task.Run(() => _listener.InitializeListener());

InputHandler _inputHandler = new InputHandler();

while (true)
{
    var input = Console.ReadLine();
    if (Enum.IsDefined(typeof(InputTypes), input))
    {
        _inputHandler.Call(input);
    }
    else
    {
        Console.WriteLine("! Invalid Input.");
    }
}