module Tokens

type Tokens =

    // Numeric types
    | INT of int
    | FLOAT of float

    // Constants
    | PI
    | E

    // Trig functions
    | SIN
    | COS
    | TAN

    // Operators
    | PLUS
    | DASH
    | ASTERISK
    | SLASH
    | CARET
    | AND
    | OR

    // Comparison
    | DBLEQUAL
    | EQUAL
    | LT
    | GR
    | LTEQ
    | GREQ

    // Misc
    | LPAREN
    | RPAREN
    | LCURLY
    | RCURLY
    | SEMI
    | BANG
    | DOT
    | COMMA
    | EOF

    // Identifier
    | ID of string

    // Booleans
    | TRUE
    | FALSE

    // Conditionals
    | IF
    | ELSE
    | WHILE

    // Strings
    | STRING of string

    // New
    | NEW

    // Functions
    | FUNCTION


