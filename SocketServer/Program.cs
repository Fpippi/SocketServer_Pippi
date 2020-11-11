using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {

            //Lister in ascolto quando si parla dei server
            //end oint: indentifica una coppia ip/porta

            //creare il mio socketlistener
            //1) specifico che versione ip
            //2) tipo di socket . stream.
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            //config: ip dove ascoltare
            IPAddress ipaddr = IPAddress.Any;

            //config:devo configurare l'endpoint
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            // IPEndPoint ipep2 = new IPEndPoint(ipaddr, 23000);
            // IPEndPoint ipep3 = new IPEndPoint(ipaddr, 23000);
            //config Bind -> collegamento
            //listenersocket lo collego all'endpoin che ho appena configurato
            listenerSocket.Bind(ipep);


            //mettere in ascolto il server.
            //parametro: il numero massimo di connesioni da mettere in coda 
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("In attesa di connesione di parte del client...");

            //istruzione bloccante
            //restituisce una variabile di tipo socket.
            Socket client = listenerSocket.Accept();
            Console.WriteLine("Client Ip:    "+ client.RemoteEndPoint.ToString());
            
            //mi attrezzo per ricevere un messagio dal client
            //siccome è di tipo stram io recevrò dal byte, o eglio un byte array
            //riceverò anche il numero di byte 
            byte[] buff = new byte[128];
            int receivedBytes = 0;


            //ricevo effettivamente dei byte
            //la funzione receive restituice il numero di byte ricevuti
            //e nel primo parametro vengono messi i byte effettivamente ricevuti
            receivedBytes = client.Receive(buff);
            Console.WriteLine("Numero di byte ricevuti: "+receivedBytes);

            //i bytes devono essere convertiti in stringa
            //parametri i bytes,da dove iniziare a convertirli 0,quanti convertirne
            string receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
            Console.WriteLine("Stringa ricevuta: "+ receivedString);


            Array.Clear(buff, 0, buff.Length);
            receivedBytes = 0;

            // crea il messaggio
            string response = "Benvenuto " + client.RemoteEndPoint.ToString() + "! Al tuo servizio!\n" +
                                "Il tuo ultimo messaggio è stato: " + receivedString;

            // lo converto in byte
            buff = Encoding.ASCII.GetBytes(response);

            //invio al client il messaggio
            client.Send(buff);

            Console.ReadLine();


        }
    }
}
