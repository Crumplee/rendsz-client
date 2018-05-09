using System;
using AsynchronousClient;
using KliensKontroller;
using UI;

public class Program
{
    public static void Main(String[] args)
    {
        SocketClient.StartClient();
        if (SocketClient.successfulConnection)
        {
            Process process = new Process();
            process.ReadAndWrite();
        }
        else
        {
            FelhasznaloiInterfesz.kiir("Nyomj meg egy gombot a kilepeshez!\n");
            FelhasznaloiInterfesz.beker();
        }
        
        SocketClient.Close();
    }
}