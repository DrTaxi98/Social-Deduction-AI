using System.Collections;
using UnityEngine;

public class Killer : Agent
{
    [Range(0f, 30f)] public float killCooldown = 20f;
    [Range(0f, 30f)] public float reportCooldown = 5f;

    private bool canKill = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(KillCooldown());
    }

    private IEnumerator KillCooldown()
    {
        yield return new WaitForSeconds(killCooldown);

        canKill = true;

        Debugger.Instance.KillCooldownDebug(this);
    }

    private void Kill(Agent other)
    {
        other.Die();
        canKill = false;

        Debugger.Instance.KillDebug(this, other);

        StartCoroutine(ReportCooldown());
    }

    private IEnumerator ReportCooldown()
    {
        CanReport = false;

        yield return new WaitForSeconds(reportCooldown);

        CanReport = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canKill)
        {
            Agent otherAgent = other.GetComponentInParent<Agent>();
            if (otherAgent != null && !otherAgent.IsDead)
            {
                Kill(otherAgent);
            }
        }
    }
}
