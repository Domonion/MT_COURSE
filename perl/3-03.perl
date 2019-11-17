#usr/bin/perl
use strict;
my %domains = ();
my $ssilka = do { local $/; <STDIN> };
while($ssilka =~ /<\s*a\b.*?href\s*=\s*"(.*?)".*?>/sg)
{
    my $domainStart = $1;
    $domainStart =~ s/(.*?:\/\/)?(.*?@)?(.*)/$3/;
    my $tillEnd = $domainStart;
    if($tillEnd =~ /^(([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)*[a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?)/){
        my $domain = $tillEnd;
        $domain =~ s/^((([a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?\.)*[a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?)|).*$/$1/;
        my $lower = lc $domain;
        $domains{$domain}++;
    }
}
my @res = sort keys %domains;
foreach(@res)
{
    print;
    print "\n";
}