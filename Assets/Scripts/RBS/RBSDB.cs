using System.Collections.Generic;

public class RBSDB
{
    private List<RBSDatum> data;

    public RBSDB() { ; }

    public void AddDatum(RBSDatum datum)
    {
        data.Add(datum);
    }

    public bool Match(IRBSQuery q)
    {
        foreach (RBSDatum d in data)
        {
            if (d.Match(q))
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        string r = "";
        foreach (RBSDatum d in data)
            r += d.ToString() + "\n";
        return r.Substring(0, r.Length - 1);
    }
}
