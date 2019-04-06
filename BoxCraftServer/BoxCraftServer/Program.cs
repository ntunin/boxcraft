using ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCraftServer
{
    class Program: ClientServer.ConsoleProgram
    {
        private BoxCraftServer server;

        public Program(string[] args) : base(args)
        {
        }

        static void Main(string[] args)
        {
            program = new Program(args);
        }

        protected override void Run()
        {
            server = new BoxCraftServer();
            var connection = new SocketConnection(null, 11020);
            Console.WriteLine($"Server running on {connection.ip.ToString()}:{connection.port}");
            server.Run(connection);
        }
    }
}
