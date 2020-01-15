grammar Lexer;

grammatix : start rule1+ token+ EOF;

start : NAME DC;
rule1 : RULE_NAME attributes? DDOT rule_body ACTION? DC;
rule_body : atom+ | atom+ SEPARATOR rule_body;
atom : RULE_NAME | TOKEN_NAME;
token : TOKEN_NAME DDOT regex DC;
regex :  UPPER_COMA ~(UPPER_COMA)* UPPER_COMA;

attributes : SB attributes_list SCB;
attributes_list : attribute attributes_end?;
attributes_end : COMMA attribute attributes_end?;
attribute : (RULE_NAME|TOKEN_NAME) (RULE_NAME|TOKEN_NAME);

fragment LOW_CHAR : 'a'..'z';
fragment UP_CHAR : 'A'..'Z';
fragment CHAR : (LOW_CHAR|UP_CHAR);
UPPER_COMA : '\'';
ESCAPED_UPPER_COMA : '\\\'';
NAME : 'grammar 'CHAR+;
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