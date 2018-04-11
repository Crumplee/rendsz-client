using System;
using System.Threading.Tasks;
using AsynchronousClient;
using Communication;

namespace ClientProcess
{
    class Process
    {
        public Process() { }
        public void ReadAndWrite()
        {
            Menu();
            string data = Console.ReadLine();
            while (data != "bye")
            {
                switch(data)
                {
                    case "2":
                        Console.Write("Text: ");
                        data = data + Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("Text: ");
                        data = data + Console.ReadLine();
                        break;
                    default:
                        break;
                }
                CommObject commObject = new CommObject(data);

                Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
                Console.WriteLine("Sent request, waiting for response");
                CommObject dResponse = tsResponse.Result;
                Console.WriteLine("Received response: " + dResponse);
                Menu();
                data = Console.ReadLine();
            }
        }

        public void Menu()
        {
            Console.WriteLine("1. Date and Time");
            Console.WriteLine("2. Reverse");
            Console.WriteLine("3. Mocking");
        }
    }
}
