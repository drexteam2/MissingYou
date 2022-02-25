using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bird : MonoBehaviour
{
    public float chaseSpeedMultiplier;
    public float maxChaseSpeed;

    private AlertRange _alertRange;
    private Animator _anim;
    private Rigidbody2D _body;

    private GameObject _target;

    private void Awake()
    {
        _alertRange ??= GetComponentInChildren<AlertRange>();
        _anim ??= GetComponent<Animator>();
        _body ??= GetComponent<Rigidbody2D>();

        _alertRange.alerted.AddListener(Alerted);
    }

    private void Start()
    {
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
        _anim.Play(clips[Random.Range(0, clips.Length)].name);
    }

    private void Alerted()
    {
        _target = PlayerController.Instance.gameObject;
    }

    private void Update()
    {
        if (_target == null) return;

        Vector2 distance = _target.transform.position - transform.position;
        _body.velocity = Vector2.ClampMagnitude(distance * chaseSpeedMultiplier, maxChaseSpeed);
    }
}
