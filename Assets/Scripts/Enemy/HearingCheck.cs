using UnityEngine;

public class HearingCheck : MonoBehaviour
{
    public float hearingRange = 6f;
    public LayerMask obstacleMask;
    public Transform player;

    public bool CanHearPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > hearingRange)
            return false;

        Vector3 dir = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, hearingRange, obstacleMask))
        {
            if (!hit.collider.CompareTag("Player"))
                return false;
        }

        return true;
    }
}
