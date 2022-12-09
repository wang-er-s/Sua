using System;

namespace Sua
{
    public partial class LuaState
    {
        public int GetTopIndex()
        {
            return LuaStack.TopIndex;
        }

        public int AbsIndex(int index)
        {
            return LuaStack.AbsIndex(index);
        }

        public bool CheckStack(int count)
        {
            LuaStack.Check(count);
            return true;
        }

        /// <summary>
        /// 弹出栈顶count个值
        /// </summary>
        /// <param name="count"></param>
        public void Pop(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                LuaStack.Pop();
            }
        }

        public void Copy(int sourceIndex, int targetIndex)
        {
            LuaStack[targetIndex] = LuaStack[sourceIndex];
        }
        
        /// <summary>
        /// 将索引处的值拷贝推入栈顶
        /// </summary>
        /// <param name="index"></param>
        public void CopyAndPush(int index)
        {
            LuaStack.Push(LuaStack[index]);
        }

        /// <summary>
        /// pop栈顶的值并替换index处的值 
        /// </summary>
        /// <param name="index"></param>
        public void PopAndReplace(int index)
        {
            LuaStack[index] = LuaStack.Pop();
        }

        /// <summary>
        /// pop栈顶的值并插入到index处
        /// </summary>
        /// <param name="index"></param>
        public void PopAndInsert(int index)
        {
            Rotate(index, 1);
        }

        public void RemoveAt(int index)
        {
            Rotate(index, -1);
            Pop();
        }

        /// <summary>
        /// 将index到top的值往栈顶方向旋转n位，如果n是负数，就朝栈底方向旋转
        /// </summary>
        public void Rotate(int index, int n)
        {
            int top = LuaStack.TopIndex - 1;
            int p = LuaStack.AbsIndex(index) - 1;
            int m;
            if (n > 0)
            {
                m = top - n;
            }
            else
            {
                m = p - n - 1;
            }
            LuaStack.Reverse(p, m);
            LuaStack.Reverse(m+1, top);
            LuaStack.Reverse(p, top);
        }

        /// <summary>
        /// 设置栈顶索引，高出的pop，少的push null
        /// </summary>
        public void SetTop(int index)
        {
            int top = LuaStack.AbsIndex(index);
            if (top < 0)
            {
                throw new Exception("栈下溢出");
            }

            int count = top - LuaStack.TopIndex;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    LuaStack.Push(new LuaValue(LuaType.Nil));
                } 
            }else if (count < 0)
            {
                for (int i = count; i < 0; i++)
                {
                    LuaStack.Pop();
                }
            }
        }
    }
}