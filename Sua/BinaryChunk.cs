using System;

namespace Sua
{
    public class BinaryChunk
    {
        public Header Header;
        public Action MainFunc;

        //public const byte[] LUA_SIGNATURE = new[] { 'b', 'L', 'u', 'a' };
        public const byte LUAC_VERSION = 0x53;

        public const byte FORMAT = 0;

        //public const byte[] LUAC_DATA = "";
        public const byte CINT_SIZE = 4;
        public const byte CSIZET_SIZE = 8;
        public const byte INSTRUCTION_SIZE = 4;
        public const byte LUA_INTEGER_SIZE = 8;
        public const byte LUA_NUMBER_SIZE = 8;
        public const long LUA_INT = 0x5678;
        public const double LUAC_NUM = 370.5;

        public const byte TAG_NIL = 0x00;
        public const byte TAG_BOOLEAN = 0x01;
        public const byte TAG_NUMBER = 0x03;
        public const byte TAG_INTEGER = 0x13;
        public const byte TAG_SHORT_STR = 0x04;
        public const byte TAG_LONG_STR = 0x14;


        public struct UpValue
        {
            public byte Instack;
            public byte Idx;
        }

        /// <summary>
        /// 局部变量
        /// </summary>
        public struct LocVar
        {
            public string VarName;

            /// <summary>
            /// 起始指令索引
            /// </summary>
            public uint StartPC;

            /// <summary>
            /// 截止指令索引
            /// </summary>
            public uint EndPC;
        }

        /// <summary>
        /// 函数原型
        /// </summary>
        public struct FuncProtoType
        {
            /// <summary>
            /// 函数的来源
            /// </summary>
            public string Source;

            /// <summary>
            /// 起始行号
            /// </summary>
            public uint LineDefined;

            /// <summary>
            /// 结束行号
            /// </summary>
            public uint LastLineDefined;

            /// <summary>
            /// 固定参数的个数
            /// </summary>
            public byte NumParams;

            /// <summary>
            /// 是否有变长参数 0否 1是
            /// </summary>
            public byte IsVararg;

            /// <summary>
            /// 寄存器数量 寄存器为栈结构
            /// </summary>
            public byte MaxStackSize;

            /// <summary>
            /// 指令表
            /// </summary>
            public uint[] Code;

            /// <summary>
            /// 常量表
            /// </summary>
            public xx Constants;

            /// <summary>
            /// 子函数原型表
            /// </summary>
            public FuncProtoType[] Protos;

            /// <summary>
            /// 行号表  行号表中的行号和指令表中的指令一一对应，分别记录每条指令在源代码中对应的行号
            /// </summary>
            public uint[] LineInfo;

            /// <summary>
            /// 局部变量表
            /// </summary>
            public LocVar[] LocVars;

            /// <summary>
            /// UpValue名列表
            /// </summary>
            public string[] UpValueNames;
        }



        Action UnDump(byte[] data)
        {
            
        }
    }
    
        public struct Header
        {
            // byte[4] 签名或者叫magic number，用来放在二进制chunk文件前，起到快速识别文件格式的作用
            public byte[] Signature;

            public byte Version;

            // 也是二进制文件的标识
            public byte Format;

            // 依旧是二进制校验 byte[6]
            public byte[] LuacData;

            // Lua虚拟机在加载二进制chunk时，会检查下面5种数据类型所占用的字节数，如果和期望数值不匹配则拒绝加载。
            public byte CIntSize;

            // c size_t大小
            public byte SizeTSize;

            // 虚拟机指令
            public byte InstructionSize;

            // lua整数大小
            public byte LuaIntegerSize;

            // lua浮点数大小
            public byte LuaNumberSize;

            // 检测大小端对齐是否一致
            public long LuacInt;

            // 检测浮点数格式
            public double LuacNum;
        }
}