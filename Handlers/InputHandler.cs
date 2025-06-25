namespace Handlers;

class InputHandler
{
    public string Event { get; set; }

    public InputHandler() { }

    public void Call(string Event)
    {
        if (Event == "end")
        {
            Environment.Exit(1);
        }
        if (Event == "status")
        {
            Status.Response();
        }
    }
}