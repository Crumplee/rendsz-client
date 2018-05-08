using System;
using AsynchronousClient;
using KliensKontroller;

public class Program
{
    public static void Main(String[] args)
    {
        SocketClient.StartClient();
        Process process = new Process();
        process.ReadAndWrite();
        SocketClient.Close();
    }
}