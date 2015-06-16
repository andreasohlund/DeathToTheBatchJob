namespace CustomerCare.Batch
{
    using System;

    internal sealed class ColoredConsole : IDisposable
    {
        readonly ConsoleColor previousColor;

        public ColoredConsole(ConsoleColor consoleColor)
        {
            previousColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = previousColor;
        }
    }
}