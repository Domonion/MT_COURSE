grammar Lexer;

grammatix : grammar_name rule_list token_list;

grammar_name : NAME DC;
rule_list : rule1 | rule1 rule_list;
rule1 : RULE_NAME DDOT atom_list DC;

//attributes : SB attributes_list SCB;
//attributes_list : attribute attributes_end?;
//attributes_end : COMMA attribute attributes_end?;
//attribute : TYPE ANY_NAME;

atom_list : atom | atom atom_list;
atom : RULE_NAME | TOKEN_NAME;

token_list : token+;
token : TOKEN_NAME DDOT REGEX DC;

REGEX : '${' .*? '}$';
fragment LOW_CHAR : 'a'..'z';
fragment UP_CHAR : 'A'..'Z';
NAME : 'grammar '(LOW_CHAR|UP_CHAR)+;
DDOT : ':';
DC : ';';
WS : [ \n\t\r]+ -> skip;
RULE_NAME : LOW_CHAR+;
TOKEN_NAME : UP_CHAR+;