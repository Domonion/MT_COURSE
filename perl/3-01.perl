#usr/bin/perl
use strict;
my $emptyStringCount = 0;
my $atLeastOneStringWritten = 0;
while(<>)
{
    s/^ *//;
    s/ *$//;
    s/ +/ /g;
    if($_ eq "\n"){
        if($atLeastOneStringWritten == 1){
                $emptyStringCount = 1;
        }
    }
    else {
        $atLeastOneStringWritten = 1;
        if($emptyStringCount == 1){
            $emptyStringCount = 0;
            print "\n";
        }
        print $_;
    }
}