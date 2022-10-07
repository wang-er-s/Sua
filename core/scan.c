#define extern_
#include "data.h"
#include "defs.h"
#include "scan.h"
#include <stdio.h>
#include <string.h>
#include <ctype.h>

// get next character from the input file.
static int next(void){
    int c;
    if(Putback)
    {
        c = Putback;
        Putback = 0;
        return c;
    }

    c = fgetc(Infile);
    if(c == '\n')
        Line++;
    return c;
}

// put back an unwanted character
static void putback(int c){
    Putback = c;
}

static int skip(void)
{
    int c;
    c = next();
    while (c == ' ' || c == '\t' || c == '\n' || c == '\r' || c == '\f')
    {
        c = next();
    }
    return c;
}

// return the position of characher c
// in string s, or -1 if c not found;
static int char_pos(char *s, int c){
    char *p;
    p = strchr(s, c);
    return p ? p - s : -1;    
}

// 为什么不简单地从c中减去ASCII值'0'，使其成为一个整数？
// 答案是，以后我们也可以做char_pos("0123456789abcdef")来转换十六进制的数字。
static int scan_int(int c){
    int k, val = 0;
    while ((k = char_pos("0123456789", c)) >= 0)
    {
        val = val * 10 + k;
        c = next();
    }
    putback(c);
    return val;
}

int scan(struct token *t){
    int c;

    // skip whitespace
    c = skip();

    //
    switch (c)
    {
    case EOF:
        return 0;
    case '+':
        t->token = T_PLUS;
        break;
    case '-':
        t->token = T_MINUS;
    break;
    case '*':
        t->token = T_STAR;
    break;
    case '/':
        t->token = T_SLASH;
    break;
    default:
        // 如果是10进制数字
        if(isdigit(c)){
            t->int_value = scan_int(c);
            t->token = T_INTLIT;
        }
        break;
    }

    return 1;
}