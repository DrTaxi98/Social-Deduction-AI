using System.Collections.Generic;

public class RBSDB
{
    private List<RBSDatum> data;

    public RBSDB()
    {
        data = new List<RBSDatum>();
    }

    public void AddDatum(RBSDatum datum)
    {
        data.Add(datum);
    }

    public List<RBSDatum> Match(IRBSQuery q)
    {
        return q.Match(data);
    }

    public override string ToString()
    {
        string r = "";
        foreach (RBSDatum d in data)
            r += d.ToString() + "\n";
        return r.Substring(0, r.Length - 1);
    }
}
