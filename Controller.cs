using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPD_Client_HW
{
    internal class Controller
    {
        public delegate void GetAnswerToForm(string answer);
        public event GetAnswerToForm AnswerToForm;


        IPAddress iPServer = IPAddress.Loopback;        //так как сервер  данный момент я  , иначе просто введем необходимый ip
        Socket socket;
        Socket socketServer;
        int port = 49152;
        [Serializable]
        public struct Message
        {
            public string mes;
            public string user;
        }




        public void WaitAnswerFromServer()              //ставим клиента на прослушкук ответа от сервера) 
        {
            try
            {
             
                while (true)
                {
                 
                    EndPoint remote = new IPEndPoint(0x7F000001, 101);                  //вопрос заключается в этом !!!!!     
                    byte[] arr = new byte[socket.ReceiveBufferSize];

                    socket.ReceiveFrom(arr, ref remote);
                   
                    string servertIp = ((IPEndPoint)remote).Address.ToString();

          

                    string msg = Encoding.UTF8.GetString(arr);

                    AnswerToForm(servertIp + " : " + msg);


                }


            }
            catch (Exception ex) { MessageBox.Show( "64 строка + "  + ex.ToString()); }

        

        }



        public string SendMessage(string text)
        {
            
            StringBuilder txt = new StringBuilder(text);
            try
            {
              

                Send(text, Connect());
             
                
                Task.Run(()=>WaitAnswerFromServer());
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
            return $" Вы: {txt} {DateTime.Now.ToShortTimeString()}";
        }



        IPEndPoint Connect()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(iPServer, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
            return iPEndPoint;
        }

         void Send(string message, IPEndPoint iPEndPoint)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, message);
            
            socket.SendTo(stream.GetBuffer(), iPEndPoint);
            stream.Close();
          
        }
        public void Disconnect()
        {
            if (socket != null)
            {

                socket.Shutdown(SocketShutdown.Send);
                socket.Close();
            }
        }






    }
}
