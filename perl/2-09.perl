while (<>) {
    s/\([^\)]*\)/\(\)/g;
    print;
}