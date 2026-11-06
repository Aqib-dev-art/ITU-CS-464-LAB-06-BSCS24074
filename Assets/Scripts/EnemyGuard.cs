using UnityEngine;

public class EnemyGuard : MonoBehaviour
{
    enum State { Patrol, Chase, Return }

    [SerializeField] Transform[] points;
    [SerializeField] float patrolSpeed = 2f, chaseSpeed = 3.8f, turnSpeed = 360f;
    [SerializeField] float sightRange = 6f;   
    [SerializeField] float loseRange = 9f;    

    State state = State.Patrol;
    int index;
    Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (points == null || points.Length == 0 || player == null) return;

        switch (state)
        {
            case State.Patrol:
                if (MoveTo(points[index].position, patrolSpeed))
                    index = (index + 1) % points.Length;
                if (DistanceToPlayer() <= sightRange) state = State.Chase;
                break;

            case State.Chase:
                MoveTo(player.position, chaseSpeed);
                if (DistanceToPlayer() > loseRange)
                {
                    index = NearestPoint();
                    state = State.Return;
                }
                break;

            case State.Return:
                if (MoveTo(points[index].position, patrolSpeed))
                {
                    index = (index + 1) % points.Length;
                    state = State.Patrol;
                }
                if (DistanceToPlayer() <= sightRange) state = State.Chase;
                break;
        }
    }

   
    bool MoveTo(Vector3 target, float speed)
    {
        target.y = transform.position.y;
        Vector3 toTarget = target - transform.position;
        if (toTarget != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(toTarget), turnSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        return transform.position == target;
    }

    float DistanceToPlayer()
    {
        Vector3 d = player.position - transform.position;
        d.y = 0f;
        return d.magnitude;
    }

    int NearestPoint()
    {
        int best = 0;
        float bestDist = float.MaxValue;
        for (int i = 0; i < points.Length; i++)
        {
            float d = Vector3.Distance(transform.position, points[i].position);
            if (d < bestDist) { bestDist = d; best = i; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
