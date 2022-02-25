using UnityEngine;
using UnityEngine.Events;

public class AlertRange : MonoBehaviour
{
    public UnityEvent alerted;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            alerted.Invoke();
        }
    }
}
