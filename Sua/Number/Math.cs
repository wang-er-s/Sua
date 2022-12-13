using System;

namespace Sua
{
    public static class MathF
    {
    
        /// <summary>
        /// 向下取整的除法
        /// </summary>
        public static long FloorDiv(long a, long b)
        {
            if (Math.Sign(a) == Math.Sign(b) || a % b == 0)
            {
                return a / b;
            }

            return a / b - 1;
        }
        
        /// <summary>
        /// 向下取整的除法
        /// </summary>
        public static double FloorDiv(double a, double b)
        {
            return Math.Floor(a / b);
        }

        /// <summary>
        /// 取模
        /// </summary>
        public static long Mod(long a, long b)
        {
            ShiftLeft(a, b);
            return a - FloorDiv(a, b) * b;
        }

        /// <summary>
        /// 取模
        /// </summary>
        public static double Mod(double a, double b)
        {
            return a - FloorDiv(a, b) * b;
        }

        /// <summary>
        /// 左移运算符
        /// </summary>
        public static long ShiftLeft(long num, long count)
        {
            if (count >= 0)
            {
                return (int)num << (int)count;
            }
            else
            {
                return ShiftRight(num, count);
            }
        }

        /// <summary>
        /// 右移运算符
        /// </summary>
        public static long ShiftRight(long num, long count)
        {
            if (count >= 0)
            {
                return (int)num >> (int)count;
            }
            else
            {
                return ShiftLeft(num, count);
            }
        }
    }

    /// <summary>
    /// lua的运算符
    /// </summary>
    public enum LuaCalcOperator
    {
        Add,
        Sub,
        Mul,
        Div,
        // %
        Mod,
        Pow,
        // 整除
        IDiv,
        And,
        Or,
        //异或
        XOr,
        Not,
        // <<
        ShL,
        // >>
        ShR,
        // -a 
        UNm,
        // ^a
        BNot,
    }

    public enum LuaCompareOperator
    {
        Equal,
        LessThan,
        LessEqual
    }
}