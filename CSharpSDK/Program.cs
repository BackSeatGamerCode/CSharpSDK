namespace CSharpSDK
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestSDK testSdk = new TestSDK();
            testSdk.Start();

            while (true)
            {
                testSdk.Poll(5, true, "1234");
            }
        }
    }
}