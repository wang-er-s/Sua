using System;
using System.IO;

namespace Sua
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TestLuaStateCalc();
            // TestLuaState();
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

        private static void TestLuaStateCalc()
        {
            LuaState ls = new LuaState();
            ls.PushInteger(1);
            ls.PushString("2.0");
            ls.PushString("3.0");
            ls.PushNumber(4.0);
            ls.LuaStack.Print();
            
            ls.Arith(LuaCalcOperator.Add);
            ls.LuaStack.Print();
            ls.Arith(LuaCalcOperator.UNm);
            ls.LuaStack.Print();
            ls.StrLen(2);
            ls.LuaStack.Print();
            ls.StrConcat(3);
            ls.LuaStack.Print();
            ls.PushBool(ls.Compare(1,2, LuaCompareOperator.Equal));
            ls.LuaStack.Print();
        }
    }
}