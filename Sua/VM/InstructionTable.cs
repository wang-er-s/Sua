namespace Sua
{
    /// <summary>
    /// 指令表，包含指令的基本信息
    /// </summary>
    public struct InstructionTable
    {
        public string Name;
        /// <summary>
        /// 编码模式
        /// </summary>
        public OpcodeMode OpcodeMode;
        /// <summary>
        /// B的操作数
        /// </summary>
        public OpArg BArgOp;
        /// <summary>
        /// C的操作数
        /// </summary>
        public OpArg CArgOp;
        /// <summary>
        /// 是否设置寄存器A
        /// </summary>
        public bool AFlag;
        /// <summary>
        /// 当前指令是一个test，下个指令必须是jump
        /// </summary>
        public bool TestFlag;

        public InstructionTable(bool testFlag, bool aFlag,OpArg bArgOp, OpArg cArgOp,OpcodeMode opcodeMode, Opcode opcode)
        {
            Name = opcode.ToString();
            OpcodeMode = opcodeMode;
            BArgOp = bArgOp;
            CArgOp = cArgOp;
            AFlag = aFlag;
            TestFlag = testFlag;
        }

        public static InstructionTable[] InstructionTables = new InstructionTable[]
        {
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IABC, Opcode.Move),
            new InstructionTable(false, true, OpArg.K, OpArg.N, OpcodeMode.IABx, Opcode.LoadK),
            new InstructionTable(false, true, OpArg.N, OpArg.N, OpcodeMode.IABx, Opcode.LoadKX),
            new InstructionTable(false, true, OpArg.U, OpArg.U, OpcodeMode.IABC, Opcode.LoadBool),
            new InstructionTable(false, true, OpArg.U, OpArg.N, OpcodeMode.IABC, Opcode.LoadNil),
            new InstructionTable(false, true, OpArg.U, OpArg.N, OpcodeMode.IABC, Opcode.GetUpValue),
            new InstructionTable(false, true, OpArg.U, OpArg.K, OpcodeMode.IABC, Opcode.GetTabup),
            new InstructionTable(false, true, OpArg.R, OpArg.K, OpcodeMode.IABC, Opcode.GetTable),
            new InstructionTable(false, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.SetTabup),
            new InstructionTable(false, false, OpArg.U, OpArg.N, OpcodeMode.IABC, Opcode.SetUpValue),
            new InstructionTable(false, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.SetTable),
            new InstructionTable(false, true, OpArg.U, OpArg.U, OpcodeMode.IABC, Opcode.NewTable),
            new InstructionTable(false, true, OpArg.R, OpArg.K, OpcodeMode.IABC, Opcode.Self),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Add),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Sub),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Mul),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Mod),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Pow),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Div),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.IDiv),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.BAnd),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.BOr),
            new InstructionTable(false, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.BXOr),
            new InstructionTable(false, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.ShL),
            new InstructionTable(false, true, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.ShR),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IABC, Opcode.Unm),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IABC, Opcode.BNot),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IABC, Opcode.Not),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IABC, Opcode.Len),
            new InstructionTable(false, true, OpArg.R, OpArg.R, OpcodeMode.IABC, Opcode.Concat),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IAsBx, Opcode.Jmp),
            new InstructionTable(true, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Eq),
            new InstructionTable(true, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Lt),
            new InstructionTable(true, false, OpArg.K, OpArg.K, OpcodeMode.IABC, Opcode.Le),
            new InstructionTable(true, false, OpArg.N, OpArg.U, OpcodeMode.IABC, Opcode.Test),
            new InstructionTable(true, true, OpArg.R, OpArg.U, OpcodeMode.IABC, Opcode.TestSet),
            new InstructionTable(false, true, OpArg.U, OpArg.U, OpcodeMode.IABC, Opcode.Call),
            new InstructionTable(false, true, OpArg.U, OpArg.U, OpcodeMode.IABC, Opcode.TailCall),
            new InstructionTable(false, false, OpArg.U, OpArg.N, OpcodeMode.IABC, Opcode.Return),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IAsBx, Opcode.ForLoop),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IAsBx, Opcode.ForPrep),
            new InstructionTable(false, false, OpArg.N, OpArg.U, OpcodeMode.IABC, Opcode.TForCall),
            new InstructionTable(false, true, OpArg.R, OpArg.N, OpcodeMode.IAsBx, Opcode.TForloop),
            new InstructionTable(false, false, OpArg.U, OpArg.U, OpcodeMode.IABC, Opcode.SetList),
            new InstructionTable(false, true, OpArg.U, OpArg.N, OpcodeMode.IABx, Opcode.Closure),
            new InstructionTable(false, true, OpArg.U, OpArg.N, OpcodeMode.IABC, Opcode.Vararg),
            new InstructionTable(false, false, OpArg.U, OpArg.U, OpcodeMode.IAx, Opcode.ExtraArg),
        };
        
    }

}