using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        PlayerController.Instance.transform.position = transform.position;
        PlayerController.Instance.GetComponent<Rigidbody2D>().gravityScale = 2.5f;
    }
}
