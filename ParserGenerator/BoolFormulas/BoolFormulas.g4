grammar BoolFormulas;
s [bool:res] : a f {$me.res = $0.res ^ $1.res;};
f [bool:res] : XOR s {$me.res = $1.res;} | EPS {$me.res = false;};
a [bool:res] : b e {$me.res = $0.res | $1.res;};
e [bool:res] : OR a {$me.res = $1.res;} | EPS {$me.res = false;};
b [bool:res] : c d {$me.res = $0.res & $1.res;};
d [bool:res] : AND b {$me.res = $1.res;} | EPS {$me.res = false;};
c [bool:res] : NOT c {$me.res = !$1.res;} | VAR {$me.res = bool.Parse($text[0]);} | OPEN s CLOSE {$me.res = $1.res;};

XOR : '\^';
OR : '\|';
AND : '&';
NOT : '!';
OPEN : '\(';
CLOSE : '\)';
VAR : '\(false\|true\)';