using System;
using System.IO;

namespace Sua
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var data = File.ReadAllBytes(@"E:\CSProject\Sua\Sua\Sample\luac.out");
            var chunk = BinaryChunk.UnDump(data);
            Console.WriteLine(chunk);
        }
    }
}