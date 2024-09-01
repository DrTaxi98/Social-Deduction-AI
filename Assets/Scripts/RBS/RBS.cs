using System.Collections.Generic;

public class RBS
{
    private List<RBSRule> rules;

    public RBS()
    {
        rules = new List<RBSRule>();
    }

    public void AddRule(RBSRule rule)
    {
        rules.Add(rule);
    }

    public void Run()
    {
        foreach (RBSRule r in rules)
        {
            if (r.Fire())
                return;
        }
    }
}
