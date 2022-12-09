namespace Sua
{
    public partial class LuaState
    {
        public void PushNil()
        {
            LuaStack.Push(new LuaValue(LuaType.Nil));
        }

        public void PushBool(bool b)
        {
            LuaStack.Push(new LuaValue(LuaType.Bool, boolean: b));
        }

        public void PushInteger(long l)
        {
            LuaStack.Push(new LuaValue(LuaType.Number, int64: l));
        }

        public void PushNumber(double d)
        {
            LuaStack.Push(new LuaValue(LuaType.Number, float64: d));
        }

        public void PushString(string s)
        {
            LuaStack.Push(new LuaValue(LuaType.String, str: s));
        }
    }
}