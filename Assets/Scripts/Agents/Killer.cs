using System.Collections;
using UnityEngine;

public class Killer : Agent
{
    [Range(1f, 30f)] public float killCooldown = 20f;
    private bool canKill = false;
    private bool hasKilled = false;

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
    }

    private void Kill(Agent other)
    {
        other.Die();
        hasKilled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canKill && !hasKilled && other.TryGetComponent(out Agent otherAgent))
        {
            Kill(otherAgent);
        }
    }
}
