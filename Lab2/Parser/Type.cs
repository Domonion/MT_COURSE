namespace Parser
{
    public enum Type : byte
    {
        LPAREN, //'('
        RPAREN, //')'
        END, //'$'
        AND, //'&'
        OR, //'|'
        XOR, //'^'
        NOT, //'!'
        VAR //[a-z]
    }
}