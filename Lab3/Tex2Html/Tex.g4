grammar Tex;

tex         : BEGIN string END #texExp;

string      : '$' expression '$' #stringExp;

expression  : expression '=' expression                     #eqExp
            | '(' expression ')'                            #parentExp
            | expression (MULTIPLY|DIVISION) expression     #mulDivExp
            | expression (PLUS|MINUS) expression            #addSubExp
            | <assoc=right> expression (UP|DOWN) expression #powExp
            | NUMBER                                        #numAtomExp
            | ID                                            #idAtomExp   
            | '-'expression                                 #unaryExp  
            ;

OPEN_BRACKET : '{';
CLOSE_BRACKET : '}';
BEGIN : '\\begin'OPEN_BRACKET'document'CLOSE_BRACKET;
END : '\\end'OPEN_BRACKET'document'CLOSE_BRACKET;
WHITESPACE : [ \n\t\r]+ -> skip;
UP : '^';
DOWN : '_';
PLUS : '+';
MINUS : '-';
DIVISION    : '\\div';
MULTIPLY    : '*';
ID     : ('a'..'z'|'A'..'Z')+;
NUMBER : [1-9][0-9]*;