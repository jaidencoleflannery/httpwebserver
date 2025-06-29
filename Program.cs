global using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Listener = Server.Listener;
using Models.Enums;
using Handlers;

Console.WriteLine("\n\n [ Starting Local HTTP Webserver ] \n\n");

Listener _httpHandler = new Listener();
var httpThread = new Thread(_httpHandler.InitializeListener);
httpThread.IsBackground = false;
httpThread.Start();

InputHandler _inputHandler = new InputHandler();
var inputThread = new Thread(_inputHandler.InitializeListener);
inputThread.IsBackground = false;
inputThread.Start();