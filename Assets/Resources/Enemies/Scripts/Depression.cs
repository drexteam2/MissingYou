using System.Collections;
using System.Linq;
using UnityEngine;

public class Depression : MonoBehaviour
{
    public float idleMin;
    public float idleMax;
    public float holdTime;
    public float airTime;
    
    private Animator _anim;
    private BoxCollider2D _col;
    private Rigidbody2D _body;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
        _col ??= transform.Find("Physical Body").GetComponent<BoxCollider2D>();
        _body ??= GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _terrainMask = 1 << LayerMask.NameToLayer("Terrain");

        StartCoroutine(AI());
    }

    private IEnumerator AI()
    {
        if (PlayerController.Instance.transform.position.x - transform.position.x > 0 && transform.localScale.x > 0 ||
            PlayerController.Instance.transform.position.x - transform.position.x < 0 && transform.localScale.x < 0)
            Flip();

        _anim.Play("Idle");
        yield return new WaitForSeconds(Random.Range(idleMin, idleMax));
        
        _anim.Play("Launch Antic");
        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Launch Antic").length);
        
        _anim.Play("Hold");
        yield return new WaitForSeconds(holdTime);

        _anim.Play("Launch");
        Vector2 selfPos = transform.position;
        Vector2 targetPos = PlayerController.Instance.transform.position;
        Vector2 delta = targetPos - selfPos;
        Vector2 launchVel = new Vector2(delta.x / airTime, (delta.y / airTime) - 0.5f * Physics2D.gravity.y * airTime * _body.gravityScale);
        _body.velocity = launchVel;

        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Launch").length);

        _anim.Play("In Air");

        yield return new WaitUntil(() => _body.velocity.y <= 0 && IsGrounded());

        _anim.Play("Land");
        _body.velocity = Vector2.zero;
        yield return new WaitForSeconds(_anim.runtimeAnimatorController.animationClips
            .First(clip => clip.name == "Land").length);

        StartCoroutine(AI());
    }

    public void Flip()
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private float _groundCheck = 0.02f;
    private int _terrainMask;
    public bool IsGrounded()
    {
        Bounds bounds = _col.bounds;
        var leftOrigin = new Vector2(bounds.min.x, bounds.min.y);
        var centerOrigin = new Vector2(bounds.center.x, bounds.min.y);
        var rightOrigin = new Vector2(bounds.max.x, bounds.min.y);
        Debug.DrawRay(leftOrigin, Vector2.down, Color.red);
        Debug.DrawRay(centerOrigin, Vector2.down, Color.red);
        Debug.DrawRay(rightOrigin, Vector2.down, Color.red);
        return Physics2D.Raycast(leftOrigin, Vector2.down, _groundCheck, _terrainMask) ||
               Physics2D.Raycast(centerOrigin, Vector2.down, _groundCheck, _terrainMask) ||
               Physics2D.Raycast(rightOrigin, Vector2.down, _groundCheck, _terrainMask);
    }
}
