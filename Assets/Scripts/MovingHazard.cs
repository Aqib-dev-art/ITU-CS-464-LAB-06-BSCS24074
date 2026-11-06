using UnityEngine;


public class MovingHazard : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float distance = 10f;
    [SerializeField] Vector3 direction = Vector3.right;

    Rigidbody rb;
    Vector3 start;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        start = rb.position;
    }

    void FixedUpdate()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);
        rb.MovePosition(start + direction.normalized * x);
    }
}
