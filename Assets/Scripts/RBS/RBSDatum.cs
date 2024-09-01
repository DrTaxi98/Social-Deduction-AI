using System;

public class RBSDatum
{
    public string id;
    public object value; // a value or another datum

    public RBSDatum(string s)
    {
        id = s;
    }

    public bool Match(IRBSQuery q)
    {
        throw new Exception("unimplemented");
    }

    public override string ToString()
    {
        return "( " + id + " " + value.ToString() + " )";
    }
}
