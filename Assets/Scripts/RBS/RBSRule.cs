public delegate bool RBSCondition();
public delegate void RBSAction();

public class RBSRule
{
    private RBSCondition Condition;
    private RBSAction Action;

    public RBSRule(RBSCondition c, RBSAction a)
    {
        Condition = c;
        Action = a;
    }

    public bool Fire()
    {
        if (Condition())
        {
            Action();
            return true;
        }
        return false;
    }
}
