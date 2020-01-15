grammar Lexer;

grammatix : start rule1+ token+;

start : NAME DC;
rule1 : RULE_NAME DDOT atom+ DC;
atom : RULE_NAME | TOKEN_NAME;
token : TOKEN_NAME DDOT REGEX DC;

//attributes : SB attributes_list SCB;
//attributes_list : attribute attributes_end?;
//attributes_end : COMMA attribute attributes_end?;
//attribute : TYPE ANY_NAME;

REGEX : '\'' .*? '\'';
fragment LOW_CHAR : 'a'..'z';
fragment UP_CHAR : 'A'..'Z';
NAME : 'grammar '(LOW_CHAR|UP_CHAR)+;
DDOT : ':';
DC : ';';
WS : [ \n\t\r]+ -> skip;
RULE_NAME : LOW_CHAR+;
TOKEN_NAME : UP_CHAR+;