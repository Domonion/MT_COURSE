grammar Calculator;

e [double:res] : t {$eval($0.res);} ee {$me.res = $1.res;};

ee [double:res] (double:res):  TIERA  t {
                                            if($text[0] == "+")
                                            {
                                                $eval(res + $1.res);
                                            }
                                            else
                                            {
                                                $eval(res - $1.res);
                                            }
                                        }
                            ee {$me.res = $2.res;}
                            | EPS       {
                                            $me.res = res;
                                        };
                                        
t [double:res] : f {$eval($0.res);} tt {$me.res = $1.res;};

tt [double:res] (double:res): TIERB f {
                                if($text[0] == "*")
                                {
                                    $eval(res * $1.res);
                                }
                                else
                                {
                                    $eval(res / $1.res);
                                }
                            } tt {$me.res = $2.res;}
                | EPS {$me.res = res;};
                
f [double:res] : NUMBER {$me.res = double.Parse($text[0]);} | OPEN e CLOSE {$me.res = $1.res;};

TIERA : '[+|-]';
TIERB : '[*|/]';
NUMBER : '[1-9][0-9]*';
OPEN : '\(';
CLOSE : '\)';
SKIP : '\s+';