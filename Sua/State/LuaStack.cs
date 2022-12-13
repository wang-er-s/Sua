using System;
using System.Text;

namespace Sua
{
    public class LuaStack 
    {
        /// <summary>
        /// 存放的数值
        /// </summary>
        private LuaValue[] slots;


        /// <summary>
        /// 栈顶索引 topIndex-1才是第一个值
        /// </summary>
        public int TopIndex { get; private set; }

        /// <summary>
        /// 检查是否还可以容纳count个值
        /// 不可以的话就扩容
        /// </summary>
        /// <param name="count"></param>
        public void Check(int count)
        {
            int free = slots.Length - TopIndex;
            int needCapacity = count - free;
            if (needCapacity > 0)
            {
                LuaValue[] tmpSlots = new LuaValue[needCapacity];
                Array.Copy(slots, 0, tmpSlots, 0, slots.Length);
                slots = tmpSlots;
            }
        }

        public void Push(LuaValue value)
        {
            if (TopIndex == slots.Length)
            {
                throw new StackOverflowException();
            }

            slots[TopIndex] = value;
            TopIndex++;
        }

        public LuaValue Pop()
        {
            if (TopIndex <= 0)
            {
                throw new Exception("栈向下溢出");
            }

            TopIndex--;
            var result = slots[TopIndex];
            slots[TopIndex] = null;
            return result;
        }

        public int AbsIndex(int index)
        {
            if (index >= 0) return index;
            return index + TopIndex + 1;
        }

        public bool IsValidIndex(int index)
        {
            index = AbsIndex(index);
            return index <= TopIndex && index > 0;
        }

        /// <summary>
        /// 索引从1开始
        /// </summary>
        public LuaValue this[int index]
        {
            get
            {
                if (!IsValidIndex(index))
                {
                    return null;
                }

                index = AbsIndex(index);
                return slots[index - 1];
            }
            set
            {
                if (!IsValidIndex(index))
                {
                    throw new IndexOutOfRangeException();
                }

                index = AbsIndex(index);
                slots[index - 1] = value;
            }
        }

        public void Reverse(int from, int to)
        {
            for (; from < to;)
            {
                (slots[from], slots[to]) = (slots[to], slots[from]);
                from++;
                to--;
            }
        }
        
        private LuaStack()
        {
        }

        public static LuaStack Create(int size)
        {
            return new LuaStack()
            {
                slots = new LuaValue[size],
                TopIndex = 0
            };
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TopIndex; i++)
            {
                sb.Append($"[{slots[i]}]");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}