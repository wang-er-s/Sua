using System;
using System.Text;

namespace Sua
{
    public partial class LuaState
    {
        /// <summary>
        /// 常见算数
        /// </summary>
        public void Arith(LuaCalcOperator op)
        {
            LuaValue a;
            LuaValue b = LuaStack.Pop();
            if (op != LuaCalcOperator.UNm && op != LuaCalcOperator.BNot)
            {
                a = LuaStack.Pop();
            }
            else
            {
                a = b;
            }

            LuaValue result = ArithInternal(a,b, op);
            if (result == null)
            {
                throw new Exception("算数计算错误");
            }
            LuaStack.Push(result);
        }

        private static LuaValue ArithInternal(LuaValue a, LuaValue b, LuaCalcOperator op)
        {
            var func = GetArithFunc(op);
            if (func.floatFunc != null)
            {
                if (a.TryToFloat(out var af))
                {
                    if (b.TryToFloat(out var bf))
                    {
                        return new LuaValue(LuaType.Number, float64: func.floatFunc(af, bf));
                    }
                    if(b.TryToInteger(out var bi))
                    {
                        return new LuaValue(LuaType.Number, float64: func.floatFunc(af, bi));
                    }
                }else if (a.TryToInteger(out var ai))
                {
                    if (b.TryToFloat(out var bf))
                    {
                        return new LuaValue(LuaType.Number, float64: func.floatFunc(ai, bf));
                    }

                    if (b.TryToInteger(out var bi))
                    {
                        return new LuaValue(LuaType.Number, float64: func.floatFunc(ai, bi));
                    }
                }
            }else if (func.intFunc != null)
            {
                if (a.TryToInteger(out var al))
                {
                    if (b.TryToInteger(out var bl))
                    {
                        return new LuaValue(LuaType.Number, int64: func.intFunc(al, bl));
                    }
                }
            }

            return null;
        }

        private static (Func<double, double, double> floatFunc, Func<long, long, long> intFunc) GetArithFunc(LuaCalcOperator op)
        {
            double FloatAdd(double d1, double d2)
            {
                return d1 + d2;
            }

            long IntAdd(long l1, long l2)
            {
                return l1 + l2;
            }

            double FloatSub(double d1, double d2)
            {
                return d1 - d2;
            }

            long IntSub(long l1, long l2)
            {
                return l1 - l2;
            }

            double FloatMul(double d1, double d2)
            {
                return d1 * d2;
            }

            long IntMul(long l1, long l2)
            {
                return l1 * l2;
            }

            double FloatDiv(double d1, double d2)
            {
                return d1 / d2;
            }

            long And(long l1, long l2)
            {
                return l1 & l2;
            }

            long Or(long l1, long l2)
            {
                return l1 | l2;
            }

            long XOr(long l1, long l2)
            {
                return l1 ^ l2;
            }

            long IntUNm(long l1, long _)
            {
                return -l1;
            }

            double FloatUNm(double d1, double _)
            {
                return -d1;
            }

            switch (op)
            {
                case LuaCalcOperator.Add:
                    return (FloatAdd, IntAdd);
                case LuaCalcOperator.Sub:
                    return (FloatSub, IntSub);
                case LuaCalcOperator.Mul:
                    return (FloatMul, IntMul);
                case LuaCalcOperator.Div:
                    return (FloatDiv, null);
                case LuaCalcOperator.Mod:
                    return (MathF.Mod, MathF.Mod);
                case LuaCalcOperator.Pow:
                    return (Math.Pow, null);
                case LuaCalcOperator.IDiv:
                    return (MathF.FloorDiv, MathF.FloorDiv);
                case LuaCalcOperator.And:
                    return (null, And);
                case LuaCalcOperator.Or:
                    return (null, Or);
                case LuaCalcOperator.XOr:
                    return (null, XOr);
                case LuaCalcOperator.Not:
                    // todo not
                    return (null, null);
                case LuaCalcOperator.ShL:
                    return (null, MathF.ShiftLeft);
                case LuaCalcOperator.ShR:
                    return (null, MathF.ShiftRight);
                case LuaCalcOperator.UNm:
                    return (FloatUNm, IntUNm);
                case LuaCalcOperator.BNot:
                    return (null, null);
            }
            return (FloatAdd, IntAdd);
        }

        /// <summary>
        /// == < <=
        /// </summary>
        public bool Compare(int index1, int index2, LuaCompareOperator op)
        {
            var a = LuaStack[index1];
            var b = LuaStack[index2];
            switch (op)
            {
                case LuaCompareOperator.Equal:
                    return Equal(a, b);
                case LuaCompareOperator.LessThan:
                    return LessThan(a, b);
                case LuaCompareOperator.LessEqual:
                    return LessEqual(a, b);
            }

            throw new Exception("非法操作符");
        }

        private static bool Equal(LuaValue l1, LuaValue l2)
        {
            switch (l1.Type)
            {
                case LuaType.None:
                    break;
                case LuaType.Nil:
                    return l2.Type == LuaType.Nil;
                case LuaType.Bool:
                    return l1.BoolVal == l2.BoolVal;
                    break;
                case LuaType.LightUserData:
                    break;
                case LuaType.Number:
                    if (l2.Type == LuaType.Number)
                        return l1.Float64Val == l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Float64Val == l2.Int64Val;
                    return false;
                case LuaType.Integer:
                    if (l2.Type == LuaType.Number)
                        return l1.Int64Val == l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Int64Val == l2.Int64Val;
                    return false;
                case LuaType.String:
                    return l1.StrVal == l2.LuaToString();
                case LuaType.Table:
                    break;
                case LuaType.Function:
                    break;
                case LuaType.UserData:
                    break;
                case LuaType.Thread:
                    break;
            }

            return l1 == l2;
        }

        private static bool LessThan(LuaValue l1, LuaValue l2)
        {
            switch (l1.Type)
            {
                case LuaType.Number:
                    if (l2.Type == LuaType.Number)
                        return l1.Float64Val < l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Float64Val < l2.Int64Val;
                    return false;
                case LuaType.Integer:
                    if (l2.Type == LuaType.Number)
                        return l1.Int64Val < l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Int64Val < l2.Int64Val;
                    return false;
            }

            throw new Exception("比较出错");
        }
        
        private static bool LessEqual(LuaValue l1, LuaValue l2)
        {
            switch (l1.Type)
            {
                case LuaType.Number:
                    if (l2.Type == LuaType.Number)
                        return l1.Float64Val <= l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Float64Val <= l2.Int64Val;
                    return false;
                case LuaType.Integer:
                    if (l2.Type == LuaType.Number)
                        return l1.Int64Val <= l2.Float64Val;
                    if (l2.Type == LuaType.Integer)
                        return l1.Int64Val <= l2.Int64Val;
                    return false;
            }

            throw new Exception("比较出错");
        }

        public void StrLen(int index)
        {
            var val = LuaStack[index];
            if (val.Type == LuaType.String)
            {
                LuaStack.Push(new LuaValue(LuaType.Integer, int64: val.StrVal.Length));
                return;
            }

            throw new Exception($"{val.Type}没有length");
        }

        /// <summary>
        /// 栈顶弹出n个元素 拼接，并推入栈顶
        /// </summary>
        /// <param name="n"></param>
        public void StrConcat(int n)
        {
            if (n == 0)
            {
                LuaStack.Push(new LuaValue(LuaType.String, str: String.Empty));
            }
            else if(n >= 2)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < n; i++)
                {
                    if (IsString(-1))
                    {
                        sb.Insert(0, LuaStack.Pop().LuaToString());
                    }
                    else
                    {
                        throw new Exception($"字符串拼接错误 {LuaStack[-1]}");
                    }
                }
                LuaStack.Push(new LuaValue(LuaType.String, str: sb.ToString()));
            }
        }
        
    }
}