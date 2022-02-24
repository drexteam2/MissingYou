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

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
