grammar BoolFormulas;
s [string:res] : a f {$me.res = $0.res + $1.res;};
f [string:res] : XOR s {$me.res = $text[0] + $1.res;} | EPS {$me.res = "";};
a [string:res] : b e {$me.res = $0.res + $1.res;};
e [string:res] : OR a {$me.res = $text[0] + $1.res;} | EPS {$me.res = "";};
b [string:res] : c d {$me.res = $0.res + $1.res;};
d [string:res] : AND b {$me.res = $text[0] + $1.res;} | EPS {$me.res = "";};
c [string:res] : NOT c {$me.res = $text[0] + $1.res;} | VAR {$me.res = $text[0];} | OPEN s CLOSE {$me.res = $text[0] + $1.res + $text[2];};

XOR : '\^';
OR : '\|';
AND : '&';
NOT : '!';
OPEN : '\(';
CLOSE : '\)';
VAR : '[a-zA-Z]';
SKIP : '\s+';