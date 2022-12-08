using System;
using System.Text;

namespace Sua
{
    public struct Instruction
    {
        private uint code;

        public Instruction(uint code)
        {
            this.code = code;
        }

        public int Opcode()
        {
            return (int)(code & 0x3f);
        }

        public void ABC(out int a, out int b, out int c)
        {
            a = (int)(code >> 6 & 0xff);
            b = (int)(code >> 14 & 0x1ff);
            c = (int)(code >> 23 & 0x1ff);
        }

        public void ABx(out int a, out int bx)
        {
            a = (int)(code >> 6 & 0xff);
            bx = (int)(code >> 14);
        }

        public void AsBx(out int a, out int sbx)
        {
            ABx(out a, out var bx);
            // 无符号整数求有符号整数 
            sbx = bx - ConstData.MaxSBx;
        }

        public void Ax(out int ax)
        {
            ax = (int)(code >> 6);
        }

        public string OpName()
        {
            return InstructionTable.InstructionTables[Opcode()].Name;
        }

        public OpcodeMode OpcodeMode()
        {
            return InstructionTable.InstructionTables[Opcode()].OpcodeMode;
        }

        public OpArg BMode()
        {
            return InstructionTable.InstructionTables[Opcode()].BArgOp;
        }

        public OpArg CMode()
        {
            return InstructionTable.InstructionTables[Opcode()].CArgOp;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{OpName()}\t\t");
            switch (OpcodeMode())
            {
                case Sua.OpcodeMode.IABC:
                    ABC(out var a, out var b, out var c);
                    sb.Append($"{a}\t");
                    if (BMode() != OpArg.N)
                    {
                        // todo ??
                        //如果操作数B或C的最高位是1就认为它表示常量表索引，按负数输出。
                        if (b > 0xff)
                        {
                            sb.Append($"{-1 - b & 0xff}\t");
                        }
                        else
                        {
                            sb.Append($"{b}\t");
                        }
                    }

                    if (CMode() != OpArg.N)
                    {
                        if (c > 0xff)
                        {
                            sb.Append($"{-1 - c & 0xff}\t");
                        }
                        else
                        {
                            sb.Append($"{c}\t");
                        }
                    }
                    break;
                case Sua.OpcodeMode.IABx:
                    ABx(out a, out var bx);
                    sb.Append($"{a}\t");
                    if (BMode() == OpArg.K)
                    {
                        sb.Append($"{-1 - bx}\t");
                    }else if (BMode() == OpArg.U)
                    {
                        sb.Append($"{bx}\t");
                    }
                    break;
                case Sua.OpcodeMode.IAsBx:
                    AsBx(out a, out var sbx);
                    sb.Append($"{a}\t{sbx}");
                    break;
                case Sua.OpcodeMode.IAx:
                    Ax(out var ax);
                    sb.Append($"{-1 - ax}");
                    break;
            }

            return sb.ToString();
        }
    }
}