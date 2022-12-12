using System;
using System.Collections.Generic;

namespace Sua
{
    public class LuaValue
    {
        public bool BoolVal { get; }
        public long Int64Val { get; }
        public double Float64Val { get; }
        public string StrVal { get; }
        public LuaType Type { get; }

        public LuaValue(LuaType type, bool boolean = default, long int64 = default, double float64 = default,
            string str = default)
        {
            this.Type = type;
            this.BoolVal = boolean;
            this.Int64Val = int64;
            this.Float64Val = float64;
            this.StrVal = str;
        }

        public bool TryToFloat(out double val)
        {
            val = 0;
            switch (Type)
            {
                case LuaType.Number:
                    val = Int64Val == default ? Float64Val : Int64Val;
                    return true;
                case LuaType.String:
                    return double.TryParse(StrVal, out val);
                default:
                    return false;
            }
        }
        
        public bool TryToInteger(out long val)
        {
            val = 0;
            switch (Type)
            {
                case LuaType.Number:
                    if (Int64Val != default)
                    {
                        val = Int64Val;
                        return true;
                    }
                    return MathF.Float2Integer(Float64Val, out val);
                case LuaType.String:
                    return long.TryParse(StrVal, out val);
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case LuaType.None:
                case LuaType.Nil:
                case LuaType.LightUserData:
                case LuaType.UserData:
                case LuaType.Thread:
                case LuaType.Function:
                    return Type.ToString();
                    break;
                case LuaType.Bool:
                    return BoolVal.ToString();
                case LuaType.Number:
                    if (Int64Val != 0) return Int64Val.ToString();
                    return Float64Val.ToString(".00");
                case LuaType.String:
                    return StrVal;
                case LuaType.Table:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}