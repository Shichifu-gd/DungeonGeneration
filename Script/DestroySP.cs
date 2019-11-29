using UnityEngine;

public class DestroySP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint")) Destroy(other.gameObject);
    }
}