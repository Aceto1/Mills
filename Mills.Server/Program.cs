using System;

namespace Mills.Server
{
    internal class Program
    {
        private static Server server;

        static void Main(string[] args)
        {
            server = new Server();
            server.StartListening();

            Console.WriteLine("Type \"q\" to quit.");

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "q")
                    break;
            }
        }
    }
}
