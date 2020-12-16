using System;
namespace BlockChainBlockcypher.ConsoleWorkers
{
    //improvment 
    //create interface and inject implementation of it in constructor

    public static class MessageHandler
    {
        public static void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
