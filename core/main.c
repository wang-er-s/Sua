#include <stdio.h>
#include "defs.h"
#define extern_
#include "data.h"
#undef extern_
#include <errno.h>

static void init();
// print out a usage if started incorrectly
static void usage(char *prog);
static void scan_file();

void main(int argc, char *argv[]){
    if(argc != 2)
        usage(argv[0]);

    init();
    if((Infile = fopen(argv[1], 'r')) == NULL){
        fprintf(stderr, "unable to open %s: %s\n", argv[1], strerror(errno));
        exit(1);
    }
    scan_file();
    exit(0);    
}

static void init(){
    Line = 1;
    Putback = '\n';
}

static void usage(char *prog){
    fprintf(stderr, "Usage: %s infile\n", prog);
    exit(1);
}
char *tok_str[] = {"+","-","*","/","intlit"};

static void scan_file()
{
    struct token t;
    while(scan(&t))
    {
        printf("Token %s", tok_str[t.token]);
        if(t.token == T_INTLIT)
            printf(", value %d", t.int_value);
        printf("\n");
    }
}