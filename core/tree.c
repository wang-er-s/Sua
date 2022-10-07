#include "defs.h"
#include <stdlib.h>
#include <stdio.h>

struct ASTNode *makeASTNode(int op, struct ASTNode *left, struct ASTNode *right, int intVal)
{
    struct ASTNode *n;

    n = (struct ASTNode *)malloc(sizeof(struct ASTNode));
    if(n == NULL)
    {
        fprintf(stderr, "Unable to malloc in makeASTNode()\n");
        exit(1);
    }

    n->operation = op;
    n->left = left;
    n->right = right;
    n->intValue = intVal;
    return n;
}

struct ASTNode *makeASTLeaf(int op, int intVal)
{
    return makeASTNode(op, NULL, NULL, intVal);
}

struct ASTNode *makeASTOneChild(int op, struct ASTNode *left, int intVal)
{
    return makeASTNode(op, left, NULL, intVal);
}