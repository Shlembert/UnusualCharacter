using UnityEngine;

public class DedLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            Debug.Log("DEDLINE");
            UIController.instance.StopGame();
        }
    }
}
