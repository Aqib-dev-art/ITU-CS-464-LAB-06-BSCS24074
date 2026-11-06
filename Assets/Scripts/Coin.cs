using UnityEngine;


public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
