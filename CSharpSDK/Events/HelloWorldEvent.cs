using System;

namespace CSharpSDK.Events
{
    public class HelloWorldEvent : BaseEvent
    {
        public override void Execute(params object[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}