using System.Collections.Generic;

public interface IRBSQuery
{
    public List<RBSDatum> Match(List<RBSDatum> data)
    {
        return data.FindAll(Predicate);
    }

    public abstract bool Predicate(RBSDatum datum);
}
