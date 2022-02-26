using UnityEngine;
using UnityEngine.Events;

public class Enemy: MonoBehaviour
{
    public GameObject bloodPrefab;
    public PlayerDamager damager;
    public int health;
    public UnityEvent<int> healthChanged;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy Damager"))
        {
            var slashEffect = collider.GetComponent<SlashEffect>();
            if (slashEffect != null)
            {
                GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                StartCoroutine(blood.GetComponent<BloodEmitter>().SpewBlood(slashEffect.direction));

                health -= slashEffect.damage;
                healthChanged.Invoke(health);
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
