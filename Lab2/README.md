# ```C``` logic expressions parser
## Description
Logic formulas. Operations: `&`, ```|```, ```^```, ```!```.
Standard priority. Parentheses can be used to change priority. 

The operands are variables with a name of one lowercase letter. Use one terminal for every variable. For every logic operation should be separate terminal.

Example: ```(!a | b) & a & (a | !(b ^ c))```
## Implementation
```C#```,  ```.NET Framework 4.7.2```, ```NUnit```
+ [Lab2.sln](Lab2.sln) - solution
+ [Parser/parser.csproj](Parser/Parser.csproj) - parser project
+ [Tests/tests.csproj](Tests/Tests.csproj) - tests project

## Grammar

```
S -> A^S
S -> A
A -> B|A
A -> A
B -> C&B
B -> C
C -> !C
C -> VAR
C -> (S)
```
ll1 grammar
```
S -> AF
F -> none
F -> ^S
A -> BE
E -> none
E -> |A
B -> CD
D -> none
D -> &B
C -> !C
C -> VAR
C -> (S)
```
first follow
var = [a-z]
$ = end of input
$ \epsilon $


|Rule|First          |Follow               |
|----|---------------|---------------------|
|S   |`!`, `(`, `VAR`|`)`, `$`             |
|F   |`none`, `^`    |`)`, `$`             |
|A   |`!`, `(`, `VAR`|`^`,`)`, `$`         | 
|E   |`none`, `\|`   |`^`,`)`, `$`         |
|B   |`!`, `(`, `VAR`|`\|`,`^`,`)`, `$`    |
|D   |`none`, `&`    |`\|`,`^`,`)`, `$`    |
|C   |`!`, `(`, `VAR`|`&`, \|`,`^`,`)`, `$`|