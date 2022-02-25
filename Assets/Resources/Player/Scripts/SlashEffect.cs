using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public int damage;

    private Animator _anim;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _anim.Play("Slash Effect");
    }

    private void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
        transform.localScale = PlayerController.Instance.transform.localScale * Mathf.Abs(transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && 
            Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z,  90 * PlayerController.Instance.transform.localScale.x)) <= Mathf.Epsilon)
        {
            PlayerController.Instance.Bounce();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
