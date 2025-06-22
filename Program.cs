global using System;
using System.Net;
using System.Threading.Tasks;
using Listener = Server.Listener;

Console.WriteLine("\n\n [ Starting Local HTTP Webserver ] \n\n");
await Listener.InitializeListener();
