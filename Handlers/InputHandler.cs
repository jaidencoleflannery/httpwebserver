using System.Collections.Generic;

using Models.Enums;
using Handlers;

namespace Handlers;

class InputHandler
{
    public string Event { get; set; }

    public InputHandler() { }

    public void InitializeListener()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (Enum.IsDefined(typeof(InputTypes), input))
            {
                Call(input);
            }
            else
            {
                Console.WriteLine($" > Invalid Input, {input} is not recognized.");
            }
        }
    }

    public void Call(string Event)
    {
        Console.WriteLine("true");
    }
}