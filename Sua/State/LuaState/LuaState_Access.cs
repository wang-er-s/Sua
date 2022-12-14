using System;

namespace Sua
{
    public partial class LuaState
    {
        public LuaType GetLuaType(int index)
        {
            if (LuaStack.IsValidIndex(index))
            {
                return LuaStack[index].Type;
            }

            return LuaType.None;
        }

        public bool IsNone(int index)
        {
            return GetLuaType(index) == LuaType.None;
        }
        
        public bool IsNil(int index)
        {
            return GetLuaType(index) == LuaType.Nil;
        }
        
        public bool IsNoneOrNil(int index)
        {
            return IsNone(index) || IsNil(index);
        }
        
        public bool IsBoolean(int index)
        {
            return GetLuaType(index) == LuaType.Bool;
        }

        public bool IsNumber(int index)
        {
            return ToNumberX(index, out _);
        }

        public double ToNumber(int index)
        {
            ToNumberX(index, out var value);
            return value;
        }


        /// <summary>
        ///  获取索引处的值并转成number
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value">转换后的值</param>
        /// <returns>是否转换成功</returns>
        public bool ToNumberX(int index, out double value)
        {
            var val = LuaStack[index];
            return val.TryToFloat(out value);
        }

        public bool IsInteger(int index)
        {
            return ToIntegerX(index, out _);
        }

        public long ToInteger(int index)
        {
            ToIntegerX(index, out var val);
            return val;
        }

        public bool ToIntegerX(int index, out long value)
        {
            var val = LuaStack[index];
            return val.TryToInteger(out value);
        }

        public bool ToBoolean(int index)
        {
            return ConvertToBoolean(LuaStack[index]);
        }

        public bool ConvertToBoolean(LuaValue value)
        {
            switch (value.Type)
            {
                case LuaType.Nil: return false;
                case LuaType.Bool: return value.BoolVal;
                default: return true;
            }
        }
        
        public bool IsString(int index)
        {
            var t = GetLuaType(index);
            return t == LuaType.String || t == LuaType.Number || t == LuaType.Integer;
        }

        public string ToString(int index)
        {
            ToStringX(index, out var str);
            return str;
        }

        public bool ToStringX(int index, out string str)
        {
            var val = LuaStack[index];
            str = String.Empty;
            switch (val.Type)
            {
                case LuaType.String:
                case LuaType.Number:
                case LuaType.Integer:
                    str = val.LuaToString();
                    return true;
                default:
                    return false;
            }
        }
    }
}