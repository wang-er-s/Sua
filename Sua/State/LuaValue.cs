using System;
using System.Collections.Generic;
using System.Globalization;

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
                    val = Float64Val;
                    return true;
                case LuaType.Integer:
                    val = Int64Val;
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
                    val = (long)Float64Val;
                    return true;
                case LuaType.Integer:
                    val = Int64Val;
                    return true;
                case LuaType.String:
                    return long.TryParse(StrVal, out val);
                default:
                    return false;
            }
        }

        public string LuaToString()
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
                    return Float64Val.ToString(CultureInfo.InvariantCulture);
                case LuaType.Integer:
                    return Int64Val.ToString();
                case LuaType.String:
                    return StrVal;
                case LuaType.Table:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
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
                case LuaType.Bool:
                    return BoolVal.ToString();
                case LuaType.Number:
                    return Float64Val.ToString(".00");
                case LuaType.Integer:
                    return Int64Val.ToString();
                case LuaType.String:
                    return $"\"{StrVal}\"";
                case LuaType.Table:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}