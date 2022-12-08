namespace Sua
{
    /// <summary>
    /// 指令编码模式
    /// </summary>
    public enum OpcodeMode : byte
    {
        // 8 9 9
        IABC,
        // 8 18
        IABx,
        // 8 18  s表示signed有符号的
        IAsBx,
        // 26
        IAx,
    }

    /// <summary>
    /// 操作码
    /// </summary>
    public enum Opcode : byte
    {
        Move,

        LoadK,
        LoadKX,
        LoadBool,
        LoadNil,

        GetUpValue,
        GetTabup,
        GetTable,

        SetTabup,
        SetUpValue,
        SetTable,

        NewTable,

        Self,
        Add,
        Sub,
        Mul,
        Mod,
        Pow,
        Div,
        IDiv,

        BAnd,
        BOr,
        BXOr,

        ShL,
        ShR,

        Unm,

        BNot,
        Not,

        Len,
        Concat,
        Jmp,
        Eq,

        Lt,
        Le,

        Test,
        TestSet,

        Call,
        TailCall,
        Return,

        ForLoop,
        ForPrep,
        TForCall,
        TForloop,

        SetList,

        Closure,

        Vararg,

        ExtraArg,

    }

    /// <summary>
    /// operation argument 操作数
    /// </summary>
    public enum OpArg : byte
    {
        /// <summary>
        ///  不表示任何信息，也就是说不会被使用
        /// </summary>
        N,
        /// <summary>
        /// used
        /// </summary>
        U,
        /// <summary>
        /// iABC寄存器索引 或者 iAsBx跳转偏移
        /// </summary>
        R,
        /// <summary>
        /// 寄存器索引 或者 常量索引
        /// </summary>
        K,
    }
}