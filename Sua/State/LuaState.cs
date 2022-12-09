namespace Sua
{
    public partial class LuaState
    {
        public LuaStack LuaStack { get; }

        public LuaState()
        {
            LuaStack = LuaStack.Create(20);
        }

    }
}