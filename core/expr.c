#include "defs.h"
#include <stdio.h>

int ConvertTokenToASTOperation(int token)
{
    switch (token)
    {
    case T_PLUS:
        return A_ADD;
    case T_MINUS:
        return A_SUB;
    case T_STAR:
        return A_MUL;
    case T_SLASH:
        return A_DIV;
    default:
        fprintf(stderr, "unkonwn token %d in ConvertTokenToASTOperation()", token);
        exit(1);
    }
}