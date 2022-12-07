using System;
using System.Text;

namespace Sua
{
    public class Reader
    {
        private byte[] data;
        private int curIndex = 0;

        public byte ReadByte()
        {
            byte result = data[curIndex];
            curIndex++;
            return result;
        }

        public byte[] ReadBytes(uint count)
        {
            byte[] bytes = new byte[count];
            Array.Copy(data, curIndex, bytes, 0, count);
            curIndex += (int)count;
            return bytes;
        }

        public uint ReadUint32()
        {
            uint result = BitConverter.ToUInt32(data, curIndex);
            curIndex += sizeof(uint);
            return result;
        }
        
        public ulong ReadUint64()
        {
            ulong result = BitConverter.ToUInt64(data, curIndex);
            curIndex += sizeof(ulong);
            return result;
        }

        public long ReadLuaInteger()
        {
            return (long)ReadUint64();
        }
        
        public long ReadLuaNumber()
        {
            long result = BitConverter.ToInt64(data, curIndex);
            curIndex += sizeof(long);
            return result;
        }

        public string ReadString()
        {
            // 短字符串 长度<253 用1byte记录长度+1
            uint size = ReadByte();
            if (size == 0)
            {
                return string.Empty;
            }

            // 长字符串
            if (size == 0xff)
            {
                // 长字符串 长度>=253 用1个size_t记录长度+1
                size = (uint)ReadUint64();
            }

            // 最后一位有/0 所以-1
            var bytes = ReadBytes(size - 1);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 读取指令表
        /// </summary>
        /// <returns></returns>
        public uint[] ReadCode()
        {
            uint[] result = new uint[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ReadUint32();
            }

            return result;
        }

        /// <summary>
        /// 读取常量表
        /// </summary>
        /// <returns></returns>
        public LuaConstant[] ReadConstants()
        {
            LuaConstant[] result = new LuaConstant[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ReadConstant();
            }

            return result;
        }

        /// <summary>
        /// 读取常量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public LuaConstant ReadConstant()
        {
            var tag = ReadByte();
            switch (tag)
            {
                case ConstData.TAG_NIL:
                    return new LuaConstant(LuaConstantType.Nil);
                case ConstData.TAG_BOOLEAN:
                    return new LuaConstant(LuaConstantType.Bool, boolean: ReadByte() != 0);
                case ConstData.TAG_INTEGER:
                    return new LuaConstant(LuaConstantType.Integer, integer: ReadLuaInteger());
                case ConstData.TAG_NUMBER:
                    return new LuaConstant(LuaConstantType.Number, number: ReadLuaNumber());
                case ConstData.TAG_SHORT_STR:
                    return new LuaConstant(LuaConstantType.ShortStr, shortStr: ReadString());
                case ConstData.TAG_LONG_STR:
                    return new LuaConstant(LuaConstantType.LongStr, shortStr: ReadString());
                default:
                    throw new Exception("出错拉");
            }
        }

        /// <summary>
        /// 读取UpValue表
        /// </summary>
        /// <returns></returns>
        public UpValue[] ReadUpValues()
        {
            UpValue[] result = new UpValue[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new UpValue()
                {
                    Instack = ReadByte(),
                    Idx = ReadByte(),
                };
            }

            return result;
        }

        /// <summary>
        /// 检查头信息
        /// </summary>
        public ChunkHeader CheckHeader()
        {
            ChunkHeader header = new ChunkHeader();

            header.Signature = ReadBytes(4);
            string sign = Encoding.UTF8.GetString(header.Signature);
            
            if (sign != ConstData.LuaSignature)
            {
                throw new Exception("签名不对");
            }

            header.Version = ReadByte();
            if (header.Version != ConstData.LuacVersion)
            {
                throw new Exception("版本不对");
            }

            header.Format = ReadByte();
            if (header.Format != ConstData.LuacFormat)
            {
                throw new Exception("格式不对");
            }

            header.LuacData = ReadBytes(6);
            string data = Encoding.UTF8.GetString(header.LuacData);
            if (data != ConstData.LuacData)
            {
                throw new Exception("luacData不对");
            }

            header.CIntSize = ReadByte();
            if (header.CIntSize != ConstData.CintSize)
            {
                throw new Exception("cint size不对");
            }

            header.SizeTSize = ReadByte();
            if (header.SizeTSize != ConstData.CsizetSize)
            {
                throw new Exception("csizet size不对");
            }

            header.InstructionSize = ReadByte();
            if (header.InstructionSize != ConstData.InstructionSize)
            {
                throw new Exception("指令size不对");
            }

            header.LuaIntegerSize = ReadByte();
            if (header.LuaIntegerSize != ConstData.LuaIntegerSize)
            {
                throw new Exception("lua整数size不对");
            }

            header.LuaNumberSize = ReadByte();
            if (header.LuaNumberSize != ConstData.LuaNumberSize)
            {
                throw new Exception("lua浮点数size不对");
            }

            header.LuacInt = ReadLuaInteger();
            if (header.LuacInt != ConstData.LuacInt)
            {
                throw new Exception("luac int不对");
            }

            header.LuacNum = ReadLuaNumber();
            if (Math.Abs(header.LuacNum - ConstData.LuacNum) > 0.0001f)
            {
                throw new Exception("luac num不对");
            }

            return header;
        }

        /// <summary>
        /// 读取行号表
        /// </summary>
        /// <returns></returns>
        public uint[] ReadLineInfo()
        {
            uint[] result = new uint[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ReadUint32();
            }

            return result;
        }

        /// <summary>
        /// 读取局部变量表
        /// </summary>
        /// <returns></returns>
        public LocVar[] ReadLocVars()
        {
            LocVar[] result = new LocVar[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new LocVar()
                {
                    VarName = ReadString(),
                    StartPC = ReadUint32(),
                    EndPC = ReadUint32()
                };
            }

            return result;
        }

        /// <summary>
        /// 读取UpValue名列表
        /// </summary>
        /// <returns></returns>
        public string[] ReadUpValueNames()
        {
            string[] result = new string[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ReadString();
            }

            return result;
        }
        
        public FuncProtoType ReadProto(string parentSource)
        {
            var source = ReadString();
            if (string.IsNullOrEmpty(source)) source = parentSource;
            FuncProtoType protoType = new FuncProtoType()
            {
                Source = source,
                LineDefined = ReadUint32(),
                LastLineDefined = ReadUint32(),
                NumParams = ReadByte(),
                IsVararg = ReadByte(),
                MaxStackSize = ReadByte(),
                Code = ReadCode(),
                Constants = ReadConstants(),
                UpValues = ReadUpValues(),
                Protos = ReadProtos(source),
                LineInfo = ReadLineInfo(),
                LocVars = ReadLocVars(),
                UpValueNames = ReadUpValueNames()
            };
            return protoType;
        }

        public FuncProtoType[] ReadProtos(string parentSource)
        {
            FuncProtoType[] result = new FuncProtoType[ReadUint32()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ReadProto(parentSource);
            }

            return result;
        }
    }
}