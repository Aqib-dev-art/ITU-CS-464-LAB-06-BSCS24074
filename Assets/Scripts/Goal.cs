using UnityEngine;


public class Goal : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.ReachGoal();
    }
}
