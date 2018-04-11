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
                    case "1":
                        //termekregisztralas
                        BehozandoTermekRegisztralas asd = new BehozandoTermekRegisztralas();
                        asd.adatokBekerese();

                        //data = data + Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Text: ");
                        data = data + Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("Text: ");
                        data = data + Console.ReadLine();
                        break;
                    case "4":
                        Console.Write("Text: ");
                        data = data + Console.ReadLine();
                        break;
                    default:
                        break;
                }
                Menu();
                data = Console.ReadLine();
            }
        }

        public void Menu()
        {
            Console.WriteLine("1. Behozando termek regisztralas");
            Console.WriteLine("2. Termekek adatainak kiirasa");
            Console.WriteLine("3. Munkarend felvitele");
            Console.WriteLine("4. Munkarend lekerdezese");
        }
    }
}
