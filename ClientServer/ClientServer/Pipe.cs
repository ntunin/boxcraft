using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServer
{
    public class Pipe
    {
        string startHeader = "[< MESSAGE]";
        string endHeader = "[MESSAGE >]";
        Socket handler;
        bool running;
        Action<string> handleMessage;
        int packageSize = 1024;
        string messageBuffer = "";

        public Pipe(Socket handler, Action<string> handleMessage)
        {
            this.handler = handler;
            this.handleMessage = handleMessage;
            var thread = new Thread(new ThreadStart(WaitMessages));
            thread.Start();
        }

        public void SendMessage(string message)
        {
            var packageMessage = startHeader + message + endHeader;
            var buffer = new byte[packageSize];
            var bytes = Encoding.UTF8.GetBytes(packageMessage);
            var size = bytes.Length;
            for (int i = 0; i < size; i+= packageSize)
            {
                Array.Copy(bytes, i, buffer, 0, Math.Min(packageSize, size - i));
                handler.Send(buffer);
            }
        }

        private void WaitMessages()
        {
            running = true;
            byte[] bytes = new byte[packageSize];
            while (running)
            {
                try
                {
                    int bytesRecieved = handler.Receive(bytes);
                    messageBuffer += Encoding.UTF8.GetString(bytes, 0, bytesRecieved);
                    while (true)
                    {
                        var startHandled = handleMessageStart();
                        var endHandled = handleMessageEnd();
                        if (!(startHandled || endHandled))
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Close();
                }
            }
        }

        public void Close()
        {
            running = false;
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private bool handleMessageStart()
        {
            if (messageBuffer.Contains(startHeader))
            {
                int index = messageBuffer.IndexOf(startHeader);
                messageBuffer = messageBuffer.Substring(index + startHeader.Length);
                return true;
            }
            return false;
        }

        private bool handleMessageEnd()
        {
            if (messageBuffer.Contains(endHeader))
            {
                int index = messageBuffer.IndexOf(endHeader);
                var message = messageBuffer.Substring(0, index);
                handleMessage(message);
                messageBuffer = messageBuffer.Substring(index + endHeader.Length);
                return true;
            }
            return false;
        }

        private void handleMessageBody(string message, out string result)
        {
            result = "";
            if (message.Length > 0)
            {
                messageBuffer += message;
            }
        }
    }
}
