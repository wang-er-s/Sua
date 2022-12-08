namespace Sua
{
    public static class ConstData
    {
        public const string LuaSignature = "Lua";
        public const byte LuacVersion = 0x53;

        public const byte LuacFormat = 0;

        public const string LuacData = "�\r\n\n";
        public const byte CintSize = 4;
        public const byte CsizetSize= 8;
        public const byte InstructionSize = 4;
        public const byte LuaIntegerSize = 8;
        public const byte LuaNumberSize = 8;
        public const long LuacInt = 0x5678;
        // 370.5
        public const double LuacNum = 4.64522;

        public const byte TAG_NIL = 0x00;
        public const byte TAG_BOOLEAN = 0x01;
        public const byte TAG_NUMBER = 0x03;
        public const byte TAG_INTEGER = 0x13;
        public const byte TAG_SHORT_STR = 0x04;
        public const byte TAG_LONG_STR = 0x14;
    }
}