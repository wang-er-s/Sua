using System;
using System.IO;

namespace Sua
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            double d1 = 3;
            MathF.Float2Integer(d1, out var l);
            Console.WriteLine(d1);
            Console.WriteLine(l);
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

        private static void TestLuaState()
        {
            LuaState luaState = new LuaState();
            luaState.PushBool(true);
            luaState.PushInteger(10);
            luaState.PushNil();
            luaState.PushString("hello");
            luaState.CopyAndPush(-4);
            luaState.LuaStack.Print();
            luaState.PopAndReplace(3);
            luaState.LuaStack.Print();
            luaState.SetTop(6);
            luaState.LuaStack.Print();
            luaState.RemoveAt(-3);
            luaState.LuaStack.Print();
            luaState.SetTop(-5);
            luaState.LuaStack.Print();
        }
    }
}