module Tokens

type Tokens =

    // Numeric types

    | INT of int        | FLOAT of float

    // Constants

    | PI                | E

    // Trig functions

    | SIN               | COS           | TAN

    // Operators

    | PLUS              | DASH          | ASTERISK

    | SLASH             | CARET

    // Misc

    | LPAREN            | RPAREN        | EOF
