// Tokens
enum{
    T_PLUS, T_MINUS, T_STAR, T_SLASH, T_INTLIT
};

// Token structure
struct token{
    int token;
    int int_value;
};