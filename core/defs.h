#ifndef _DEFS_H
#define _DEFS_H
// Tokens
enum{
    T_PLUS, T_MINUS, T_STAR, T_SLASH, T_INTLIT
};

// Token structure
struct token{
    int token;
    int int_value;
};

enum{
    A_ADD, A_SUB, A_MUL, A_DIV, A_INT
};

struct ASTNode
{
    int operation;
    struct ASTNode *left;
    struct ASTNode *right;
    int intValue; // 对于 A_INT类型，这个是值
};

#endif