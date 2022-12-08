using System;
using System.IO;

namespace Sua
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestInstruction();
        }

        private static void TestChunk()
        {
            var data = File.ReadAllBytes(@"E:\CSProject\Sua\Sua\Sample\luac.out");
            var chunk = BinaryChunk.UnDump(data);
            Console.WriteLine(chunk.MainFunc);
        }

        private static void TestInstruction()
        {
            var data = File.ReadAllBytes(@"E:\CSProject\Sua\Sua\Sample\luac.out");
            var chunk = BinaryChunk.UnDump(data);
            foreach (var u in chunk.MainFunc.Code)
            {
                Console.WriteLine(new Instruction(u));
            }
        }
    }
}