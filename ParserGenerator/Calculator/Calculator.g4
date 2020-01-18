grammar Calculator;

e [double:res] : t ee {$me.res = $0.res + $1.res;};
ee [double:res]: TIERA t ee {
                                if($text[0] == "+")
                                {
                                    $me.res = $1.res + $2.res;
                                }
                                else
                                {
                                    $me.res = -$1.res + $2.res;
                                }
                            }
                | EPS {$me.res = 0;};
t [double:res] : f tt {$me.res = $0.res * $1.res;};
tt [double:res]: TIERB f tt {
                                if($text[0] == "*")
                                {
                                    $me.res = $1.res * $2.res;
                                }
                                else
                                {
                                    $me.res = 1 / $1.res * $2.res;
                                }
                            }
                | EPS {$me.res = 1;};
f [double:res] : NUMBER {$me.res = double.Parse($text[0]);} | OPEN e CLOSE {$me.res = $1.res;};

TIERA : '[+|-]';
TIERB : '[*|/]';
NUMBER : '[1-9][0-9]*';
OPEN : '\(';
CLOSE : '\)';
SKIP : '\s+';