using System;
using System.Text;

namespace Sua
{
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

        public override string ToString()
        {
            return VarName;
        }
    }

    public class BinaryChunk
    {
        public ChunkHeader ChunkHeader { get; private set; }
        public byte UpValueSize { get; private set; }
        public FuncProtoType MainFunc { get; private set; }

        private BinaryChunk()
        {
        }

        public static BinaryChunk UnDump(byte[] data)
        {
            BinaryChunk result = new BinaryChunk();
            Reader reader = new Reader(data);
            // 校验头部
            result.ChunkHeader = reader.CheckHeader();
            // 跳过UpValue数量
            result.UpValueSize = reader.ReadByte();
            // 读取函数原型
            result.MainFunc = reader.ReadProto(string.Empty);
            return result;
        }

        public override string ToString()
        {
            return MainFunc.ToString();
        }
    }

    /// <summary>
    /// 函数原型
    /// </summary>
    public class FuncProtoType
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
        public LuaConstant[] Constants;

        /// <summary>
        /// 子函数原型表
        /// </summary>
        public FuncProtoType[] FuncProtoTypes;

        /// <summary>
        /// UpValue表
        /// </summary>
        public UpValue[] UpValues;

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                $"{nameof(Source)}: {Source}, {nameof(LineDefined)}: {LineDefined}, {nameof(LastLineDefined)}: {LastLineDefined}, {nameof(NumParams)}: {NumParams}");
            sb.AppendLine();
            sb.AppendLine($"{nameof(Constants)}: \n\t{string.Join<LuaConstant>("\n\t", Constants)}");
            sb.AppendLine();
            sb.AppendLine("UpValues");
            for (int i = 0; i < UpValueNames.Length; i++)
            {
                sb.AppendLine($"\t{UpValueNames[i]}\t{UpValues[i].Idx}\t{UpValues[i].Instack}");
            }
            sb.AppendLine();
            sb.AppendLine($"{nameof(LocVars)}:\n\t{string.Join("\n\t", LocVars)}");
            sb.AppendLine();
            sb.AppendLine($"\n{nameof(FuncProtoTypes)}:\n {string.Join<FuncProtoType>("\n", FuncProtoTypes)}");
            return sb.ToString();
        }
    }

    public class ChunkHeader
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