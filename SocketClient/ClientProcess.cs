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
                        BehozandoTermekRegisztralas beTermekReg = new BehozandoTermekRegisztralas();
                        beTermekReg.adatokBekerese();
                        break;
                    case "2":
                        //termekekkilistazasa
                        termekekListazasa();
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

        public void termekekListazasa()
        {
            CommObject commObject = new CommObject("termekekListazasa");

            Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
            Console.WriteLine("Sent request, waiting for response");
            CommObject dResponse = tsResponse.Result;

            foreach(CommObject.termekAdatokStruct termek in dResponse.termekAdatokLista)
            {
                Console.WriteLine(termek.megrendeloAzonosito);
                Console.WriteLine(termek.termekNev);
                Console.WriteLine(termek.kulsoVonalkod);
                Console.WriteLine(termek.tipus);
                Console.WriteLine(termek.beIdopont);
                Console.WriteLine(termek.kiIdopont);
                Console.WriteLine(termek.mennyiseg);
                foreach(string raklap in termek.raklapAdatok)
                {
                    Console.WriteLine(raklap);
                }
                Console.WriteLine("________________________________");
            }
        }
    }
}
