using UnityEngine;

public class RBSAgent : MonoBehaviour
{
    private RBS rbs;

    // Start is called before the first frame update
    void Start()
    {
        rbs = new RBS();
        DefineRules();
    }

    public void TakeTurnInMeeting()
    {
        rbs.Run();
    }

    private void DefineRules()
    {
        RBSRule r = new RBSRule(Condition, Action);
        rbs.AddRule(r);
    }

    // Conditions

    private bool Condition()
    {
        return true;
    }

    // Actions

    private void Action()
    {

    }
}
