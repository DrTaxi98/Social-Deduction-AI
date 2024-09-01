using System.Collections.Generic;
using System.Linq;

public interface IRBSQuery
{
    public bool Match(List<RBSDatum> data)
    {
        return Query(data).Any();
    }

    protected abstract IEnumerable<object> Query(List<RBSDatum> data);
}
