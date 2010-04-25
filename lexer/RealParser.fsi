module Parser
// Signature file for parser generated by fsyacc
type token = 
  | NEWTok
  | FUNCTIONTok
  | STRINGTok of (string)
  | IFTok
  | ELSETok
  | WHILETok
  | TRUETok
  | FALSETok
  | IDTok of (string)
  | BANGTok
  | DOTTok
  | COMMATok
  | LPARENTok
  | RPARENTok
  | LCURLYTok
  | RCURLYTok
  | CARETTok
  | DOUBLEEQTok
  | EOFTok
  | SEMICOLONTok
  | GTTok
  | LTTok
  | GEQTok
  | LEQTok
  | EQTok
  | ANDTok
  | ORTok
  | PLUSTok
  | MINUSTok
  | TIMESTok
  | DIVTok
  | DOUBLETok of (double)
  | INTTok of (int)
type tokenId = 
    | TOKEN_NEWTok
    | TOKEN_FUNCTIONTok
    | TOKEN_STRINGTok
    | TOKEN_IFTok
    | TOKEN_ELSETok
    | TOKEN_WHILETok
    | TOKEN_TRUETok
    | TOKEN_FALSETok
    | TOKEN_IDTok
    | TOKEN_BANGTok
    | TOKEN_DOTTok
    | TOKEN_COMMATok
    | TOKEN_LPARENTok
    | TOKEN_RPARENTok
    | TOKEN_LCURLYTok
    | TOKEN_RCURLYTok
    | TOKEN_CARETTok
    | TOKEN_DOUBLEEQTok
    | TOKEN_EOFTok
    | TOKEN_SEMICOLONTok
    | TOKEN_GTTok
    | TOKEN_LTTok
    | TOKEN_GEQTok
    | TOKEN_LEQTok
    | TOKEN_EQTok
    | TOKEN_ANDTok
    | TOKEN_ORTok
    | TOKEN_PLUSTok
    | TOKEN_MINUSTok
    | TOKEN_TIMESTok
    | TOKEN_DIVTok
    | TOKEN_DOUBLETok
    | TOKEN_INTTok
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_Prog
    | NONTERM_Expr
    | NONTERM_Term
    | NONTERM_Factor
/// This function maps integers indexes to symbolic token ids
val tagOfToken: token -> int

/// This function maps integers indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val start : (Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> ( AST.exp ) 
