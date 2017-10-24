using System;

namespace SmartLog.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write($"Message: ");
            var message = Console.ReadLine();
            $"{message}".Message();
            Logger.Instance.Dispose();
        }
    }
}