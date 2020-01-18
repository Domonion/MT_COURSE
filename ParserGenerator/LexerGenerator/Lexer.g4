grammar Lexer;

grammatix : start rule1+ token+ EOF;

start : NAME DC;
rule1 : RULE_NAME attributes? inher_attributes? DDOT rule_body DC;
rule_body : atom+ ACTION? | atom+ ACTION? SEPARATOR rule_body;
atom : ACTION? RULE_NAME | TOKEN_NAME;
token : TOKEN_NAME DDOT regex DC;
regex :  UPPER_COMA ~(UPPER_COMA)* UPPER_COMA;

attributes : SB attributes_list SCB;
inher_attributes : OPEN attributes_list CLOSE;
attributes_list : attribute attributes_end?;
attributes_end : COMMA attribute attributes_end?;
attribute : (RULE_NAME|TOKEN_NAME) DDOT (RULE_NAME|TOKEN_NAME);

fragment LOW_CHAR : 'a'..'z';
fragment UP_CHAR : 'A'..'Z';
fragment CHAR : (LOW_CHAR|UP_CHAR);
UPPER_COMA : '\'';
ESCAPED_UPPER_COMA : '\\\'';
NAME : 'grammar 'CHAR+;
OPEN : '(';
CLOSE : ')';
DDOT : ':';
DC : ';';
COMMA : ',';
SB : '[';
SCB : ']';
WS : [ \n\t\r]+ -> skip;
RULE_NAME : LOW_CHAR CHAR*;
TOKEN_NAME : UP_CHAR CHAR*;
SEPARATOR : '|';
ANY : .;
ACTION : '{' ( ACTION | ~[{}] )* '}' ;