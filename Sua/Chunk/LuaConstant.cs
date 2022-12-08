using System;

namespace Sua
{
    public enum LuaConstantType
    {
        Nil,
        Bool,
        Integer,
        Number,
        ShortStr,
        LongStr,
    }
    
    public class LuaConstant
    {
    
        private LuaConstantType type;

        private bool Boolean;

        private long Integer;

        private float Number;

        private string ShortStr;
        
        private string LongStr;

        public LuaConstant(LuaConstantType type, bool boolean = default, long integer = default, float number = default, string shortStr = default, string longStr = default)
        {
            this.type = type;
            Boolean = boolean;
            Integer = integer;
            Number = number;
            ShortStr = shortStr;
            LongStr = longStr;
        }
        
        public override string ToString()
        {
            string s = String.Empty;
            switch (type)
            {
                case LuaConstantType.Nil:
                    break;
                case LuaConstantType.Bool:
                s = Boolean.ToString();
                    break;
                case LuaConstantType.Integer:
                s = Integer.ToString();
                    break;
                case LuaConstantType.Number:
                s = Number.ToString(".000");
                    break;
                case LuaConstantType.ShortStr:
                s = ShortStr;
                    break;
                case LuaConstantType.LongStr:
                s = LongStr;
                    break;
            }

            return $"{type} : {s}";
        }
    }
}