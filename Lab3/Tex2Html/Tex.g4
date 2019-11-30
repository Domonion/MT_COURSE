grammar Tex;

tex         : '$' expression '$' #texExp;

expression  : '(' expression ')'                            #parenthesisExp
            | expression (MULTIPLY|DIVISION) expression     #mulDivExp
            | expression (PUS|MINUS) expression             #addSubExp
            | <assoc=right> expression (UP|DOWN) expression #powerExp
            | NAME '(' expression ')'                       #functionExp
            | NUMBER                                        #numericAtomExp
            | ID                                            #idAtomExp     

WHITESPACE  : [ \n\t\r]+ -> skip;
DIVISION    : '\\divide';
MULTIPLY    : '*';
LITERAL     : ('a'..'z'|'A'..'Z'|'0'..'9')+;