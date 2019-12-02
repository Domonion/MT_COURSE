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


BEGIN : '\\begin{document}';
END : '\\end{document}';
WHITESPACE : [ \n\t\r]+ -> skip;
UP : '^';
DOWN : '_';
PLUS : '+';
MINUS : '-';
DIVISION    : '\\div';
MULTIPLY    : '*';
LITERAL     : ('a'..'z'|'A'..'Z'|'0'..'9')+;
NUMBER : [1-9][0-9]*;
ID : [a-zA-Z]+;
