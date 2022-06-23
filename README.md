# CSharpSDK
The generic SDK for games written in C#

## Setup
This SDK requires [BackSeatGamer Reverse Proxy](https://github.com/BackSeatGamerCode/ReverseProxy) to be running in `TCP/IP Broadcast` Mode using `JSON` format. By default, port 29175 will be used. In the future, we plan on adding a way for the user to change this if necessary.

This SDK was designed to be as generic as possible, for any game with modding in C#, however, more specific SDKs for various games may be released soon. When they are complete, they will be listed here. Feel free to fork the repository to create a more specific C# SDK.

## Usage
Due to the generic nature of this SDK, instructions on how to install mods, set up the working environment, and resources for more information can not be included.
All you need is the code included in the repository (except for `Program.cs` and `TestSDK.cs`). Be sure to also include the `Events` and `Models` directories.
While copying a library source code is by no means the best way to add a C# dependency, this is the only supported method at this time.

### Setup
Once everything is set up, development can begin! To start, simply create a new class which extends `BsgCSharpSdk`. It will require `OnRedemptionReceived` and `GetEvent` to be implemented like so:
```c#
using System;
using CSharpSDK.Events;
using CSharpSDK.Models;

namespace CSharpSDK
{
    public class TestSDK : BsgCSharpSdk
    {
        protected override void OnRedemptionReceived(Redemption redemption, params object[] args)
        {
            Console.WriteLine(redemption.ToMessage());
        }

        protected override BaseEvent GetEvent(Redemption redemption)
        {
            return new HelloWorldEvent();
        }
    }
}
```

`OnRedemptionReceived` is called whenever a person redeems a reward. You can simply omit the print statement within the method for no action to be taken. This method is called BEFORE the event is executed. The second parameter (`params object[] args`) will be explained soon.
The `ToMessage` method of the reward is called, which returns a string in the format `{guest} has redeemed the reward {name}`. This method is called in the main thread of the application.

The next method `GetEvent` is responsible for converting a redemption to an event. This would generally be implemented via case/switch statement. Nothing too fancy here.
This section uses the Gang of Four's Command pattern. The method should return the object, and so the signature of the constructor can be whatever you want it to be. Feel free to call as many methods of the object as you wish.
All you need to do is return the object when it is ready. This method is called in the main thread of the application.

### Implementation
To run the mod functionality, simply instantiate your class which extends the `BsgCSharpSdk` class. On each game loop/tick, simply call the `Poll` method of the object. Easy. 
The `Poll` method does not perform any networking or other laborious activities, so it is safe to call every game loop without performance taking a hit.

The `Poll` method can take a variable number of arguments. These arguments are passed to all methods which have a parameter `params object[] args` (`OnRedemptionReceived` in your mod class which extends `BsgCSharpSdk`, and `Execute` in the custom `Event` class).

### Custom Events
To create a custom event, simply create a class which extends `BaseEvent`. This abstract class requires the implementation of the `Execute` method, which takes `params object[] args` as its only argument (see above for explanation).

Feel free to add a constructor and other supporting methods. You will be responsible for instantiating the object in the `GetEvent` method of your class which extends the `BsgCSharpSdk` class.

The actual execution of the event should be implemented in the `Execute` method which will always be run in the main thread of the application.

The following is the source code of the built-in `HelloWorldEvent`, which prints `Hello, World!` to the console when executed:
```c#
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
```

### The `Redemption` Object
The `Redemption` Object has three getters, `GetCommand()` which returns the command of the reward, `GetName()` returns the display name of the reward, and `GetGuest()` returns the name of the guest who redeemed the reward.

The `Redemption` Object also has a method called `ToMessage` which returns a string in the format `{guest} has redeemed the reward {name}`. 

## Issues/Feedback
If you encounter any problems, or have suggestions for future updates, feel free to leave them over in the [Issue Tracker](https://github.com/BackSeatGamerCode/C#SDK/issues). Alternatively, if you have questions or want to discuss something with your fellow C# modders, then check out our [Discussions](https://github.com/BackSeatGamerCode/C#SDK/discussions). Thank you for using C# modding SDK, and good luck with your mod!