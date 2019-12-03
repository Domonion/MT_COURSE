# `TEX` to `HTML` convertor
## Description
This program is able to convert some subset of `TEX` to `MathMl`. Supports 
document in the following grammar.
```$antlr
tex         : \begin{document}
                 $ expression $
              \end{document}                            
expression  : expression '=' expression                                                 
            | '(' expression ')'                                                        
            | expression (*|\\div) expression                                 
            | expression (-|+) expression                                        
            | expression _ { expression } ^ { expression } 
            | expression ^ { expression } _ { expression }  
            | expression (^|_) { expression }   
            | [1-9][0-9]*                                                                    
            | ('a'..'z'|'A'..'Z')+                                                                           
            | '-'expression                                                              
            ;
```
## Implementation
`ANTLR` based converter. `C#`, `.NET Framework 4.7.2`, `NUnit`. 