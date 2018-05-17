using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using AsynchronousClient;
using KliensKontroller;

class LogMegtekintes
{
    public void logokListazasa()
    {
        CommObject commObject = new CommObject("logokListazasa");

        Task<CommObject> tsResponse = SocketClient.SendRequest(commObject);
        FelhasznaloiInterfesz.kiir("Sent request, waiting for response\n");
        CommObject response = tsResponse.Result;

        int i = 1;
        if (response.lista.Count > 0)
        {
            FelhasznaloiInterfesz.kiir("Logok: \n");
            foreach (string log in response.lista)
            {
                FelhasznaloiInterfesz.kiir(log + "\n");
            }
        }
        else
            FelhasznaloiInterfesz.kiir("Nincsenek logok!");
    }
}
