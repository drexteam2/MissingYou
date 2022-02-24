using UnityEngine;
using UnityEngine.Events;

public class Enemy: MonoBehaviour
{
    public PlayerDamager damager;
    public int health;
    public UnityEvent<int> healthChanged;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Triggered by: " + collider.name);
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy Damager"))
        {
            var slashEffect = collider.GetComponent<SlashEffect>();
            Debug.Log("Slash effect null? " + (slashEffect == null));
            if (slashEffect != null)
            {
                health -= slashEffect.damage;
                healthChanged.Invoke(health);
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
