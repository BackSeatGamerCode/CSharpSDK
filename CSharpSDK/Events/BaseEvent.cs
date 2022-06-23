namespace CSharpSDK.Events
{
    public abstract class BaseEvent
    {
        public abstract void Execute(params object[] args);
    }
}