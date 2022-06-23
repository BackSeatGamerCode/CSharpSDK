using System;
using CSharpSDK.Events;
using CSharpSDK.Models;

namespace CSharpSDK
{
    public class TestSDK : BsgCSharpSdk
    {
        protected override void OnRedemptionReceived(Redemption redemption, params object[] args)
        {
            Console.WriteLine(redemption.toMessage());
        }

        protected override BaseEvent GetEvent(Redemption redemption)
        {
            return new HelloWorldEvent();
        }
    }
}