#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <fcntl.h>

int token;
char *src, *old_src;
int pool_size;
int line;

// 虚拟机内存地址
// 代码段
int *text;
int *old_text;
    // 栈
int *stack;
// 数据段
char *data;

// 寄存器
// program counter 程序计数器，它存放的是一个内存地址，该地址中存放着 下一条 要执行的计算机指令
int *pc;
// base pointer 基址指针 也是用于指向栈的某些位置，在调用函数时会使用到它
int *bp;
// stack pointer 永远指向当前的栈顶 注意的是由于栈是位于高地址并向低地址增长的，所以入栈时 SP 的值减小,出栈时SP的值增加
int *sp;
// 通用寄存器， 用于存放一条指令执行后的结果
int ax;
int cycle;

// 指令集
enum { LEA ,IMM ,JMP ,CALL,JZ  ,JNZ ,ENT ,ADJ ,LEV ,LI  ,LC  ,SI  ,SC  ,PUSH,
       OR  ,XOR ,AND ,EQ  ,NE  ,LT  ,GT  ,LE  ,GE  ,SHL ,SHR ,ADD ,SUB ,MUL ,DIV ,MOD ,
       OPEN,READ,CLOS,PRTF,MALC,MSET,MCMP,EXIT };

void next()
{
    token = *src++;
    return;
}

void expression(int level)
{

}

void program()
{
    next();
    while (token > 0)
    {
        printf("token is: %c\n", token);
        next();
    }
}

int eval()
{
    int op = *pc;
    int *tmp;
    while (1)
    {
        switch(op){
            // 把值放到寄存器上
            case IMM:
                ax = *pc++;
                break;
            // 加载一个char到寄存器，地址在寄存器上
            case LC:
                ax = *(char *)ax;
                break;
            // 加载一个int到寄存器，地址在寄存器上
            case LI:
                ax = *(int *)ax;
                break;
            // 为什么 SI/SC 指令中，地址存放在栈中，而 LI/LC 中，地址存放在 ax 中？
            // 原因是默认计算的结果是存放在 ax 中的，而地址通常是需要通过计算获得，
            // 所以执行 LI/LC 时直接从 ax 取值会更高效。
            // 将寄存器中的char存放到栈顶的地址中
            case SC:
                *(char *)*sp++ = ax;
                break;
            // 将寄存器中的int存放到栈顶的地址中
            case SI:
                *(int *)*sp++ = ax;
                break;
            // 将ax放入栈中
            case PUSH:
                *--sp = ax;
                break;
            // JMP <addr> 是跳转指令，无条件地将当前的 PC 寄存器设置为指定的 <addr>
            // 需要注意的是，pc 寄存器指向的是 下一条 指令。所以此时它存放的是 JMP 指令的参数，即 <addr> 的值。
            case JMP:
                pc = (int *)*pc;
                break;
            // 为了实现 if 语句，我们需要条件判断相关的指令。这里我们只实现两个最简单的条件判断，即结果（ax）为零或不为零情况下的跳转。
            case JZ:
                pc = ax ? pc + 1 : (int *)*pc;
                break;
            case JNZ:
                pc = ax ? (int *)*pc : pc + 1;
                break;
            case CALL:
                // 存储当前的执行位置
                *--sp = (int)(pc + 1);
                // 跳转到下一个需要执行的位置
                pc = (int *)*pc;
                break;
            // ENT <size> 保存当前的栈指针，同时在栈上保留一定的空间，用以存放局部变量。
            case ENT:
                *--sp = (int)bp;
                bp = sp;
                sp = sp - *pc++;
                break;
            // ADJ <size> 用于实现 ‘remove arguments from frame’。在将调用子函数时压入栈中的数据清除
            case ADJ:
                sp = sp + *pc++;
                break;
            // 跳出子方法
            case LEV:
                sp = bp;
                bp = (int *)*sp++;
                pc = (int *)*sp++;
                break;
            // LEA <offset>
            case LEA:
                ax = (int)(bp + *pc++);
                break;
            // 每个运算符都是二元的，即有两个参数，第一个参数放在栈顶，第二个参数放在 ax 中
            // 计算后会将栈顶的参数退栈，结果存放在寄存器 ax 中
            case OR:
                ax = *sp++ | ax;
                break;
            case XOR:
                ax = *sp++ ^ ax;
                break;
            case AND:
                ax = *sp++ & ax;
                break;
            case EQ:
                ax = *sp++ == ax;
                break;
            case NE:
                ax = *sp++ != ax;
                break;
            case LT:
                ax = *sp++ < ax;
                break;
            case LE:
                ax = *sp++ <= ax;
                break;
            case GT:
                ax = *sp++ > ax;
                break;
            case GE:
                ax = *sp++ >= ax;
                break;
            case SHL:
                ax = *sp++ << ax;
                break;
            case SHR:
                ax = *sp++ >> ax;
                break;
            case ADD:
                ax = *sp++ + ax;
                break;
            case SUB:
                ax = *sp++ - ax;
                break;
            case MUL:
                ax = *sp++ * ax;
                break;
            case DIV:
                ax = *sp++ / ax;
                break;
            case MOD:
                ax = *sp++ % ax;
                break;
            // 编译器中我们需要用到的函数有：exit, open, close, read, printf, malloc, memset 及 memcmp
            // 加个内置方法支持
            case EXIT:
                printf("exit(%d)", *sp);
                return *sp;
            case OPEN:
                ax = open((char *)sp[1], sp[0]);
                break;
            case CLOS:
                ax = close(*sp);
                break;
            case READ:
                ax = read(sp[2], (char *)sp[1], sp[0]);
                break;
            case PRTF:
                tmp = sp + pc[1];
                ax = printf((char *)tmp[-1], tmp[-2], tmp[-3], tmp[-4], tmp[-5], tmp[-6]);
                break;
            case MALC:
                ax = (int) malloc(*sp);
                break;
            case MSET:
                ax = (int) memset((char *)sp[2], sp[1], *sp);
                break;
            case MCMP:
                ax = memcmp((char *)sp[2], (char *)sp[1], *sp);
                break;
            default:
                printf("未知指令集:%d\n", op);
                return -1;
        }
    }
    
    return 0;
}

int main(int argc, char **argv)
{
    int i, fd;
    argc --;
    argv ++;
    pool_size = 256 * 1024;
    line = 1;

//    if((fd = open(*argv, 0)) < 0)
//    {
//        printf("could not open(%s)\n", *argv);
//        return -1;
//    }

    if(!(src = old_src = malloc(pool_size)))
    {
        printf("could not malloc(%d) for source area\n", pool_size);
        return -1;
    }

//    if((i = read(fd, src, pool_size - 1)) <= 0)
//    {
//        printf("read() returned %d\n", i);
//        return -1;
//    }

    src[i] = 0;
//    close(fd);

    // 给虚拟机分配内存
    if(!(text = old_text = malloc(pool_size)))
    {
        printf("could not malloc(%d) for text area\n", pool_size);
        return -1;
    }

    if(!(data = malloc(pool_size)))
    {
        printf("could not malloc(%d) for data area\n", pool_size);
        return -1;
    }

    if(!(stack = malloc(pool_size)))
    {
        printf("could not malloc(%d) for stack area\n", pool_size);
        return -1;
    }

    memset(text, 0, pool_size);
    memset(data, 0, pool_size);
    memset(stack, 0, pool_size);

    // 初始化bp sp 默认是指向栈顶的
    bp = sp = (int *)((int)stack + pool_size);

    i = 0;
    text[i++] = IMM;
    text[i++] = 10;
    text[i++] = PUSH;
    text[i++] = IMM;
    text[i++] = 20;
    text[i++] = ADD;
    text[i++] = PUSH;
    text[i++] = EXIT;
    pc = text;

    program();
    return eval();
}