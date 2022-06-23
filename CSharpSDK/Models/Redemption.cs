namespace CSharpSDK.Models
{
    public class Redemption
    {
        public string command { get; set; }
        public string name { get; set; }
        public string guest { get; set; }
        
        public string GetCommand(){
            return command;
        }
        public string GetName(){
            return name;
        }
        public string GetGuest(){
            return guest;
        }

        public override string ToString()
        {
            return "Redemption{" +
                   "command='" + command + '\'' +
                   ", name='" + name + '\'' +
                   ", guest='" + guest + '\'' +
                   '}';
        }
        
        public string ToMessage() {
            return guest + " has redeemed the reward " + name;
        }

    }
}