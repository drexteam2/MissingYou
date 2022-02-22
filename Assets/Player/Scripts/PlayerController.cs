using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance => _instance;

    private Animator _anim;
    private Collider2D _col;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;

    private PlayerInputActions _input;

    private float _moveX;

    public float walkSpeed;
    public float runSpeed;

    public Interactive interactingWith;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
        _col ??= GetComponent<Collider2D>();
        _body ??= GetComponent<Rigidbody2D>();
        _sprite ??= GetComponent<SpriteRenderer>();

        _input ??= new PlayerInputActions();
        _instance ??= this;
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Interact.performed += Interact;
        _input.Player.Movement.performed += Move;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Player.Interact.performed -= Interact;
        _input.Player.Movement.performed -= Move;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && interactingWith != null)
        {
            var typewriter = UIManager.Instance.GetComponentInChildren<Typewriter>();
            StartCoroutine(typewriter.StartTyping(interactingWith.interactText));
        }
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveX = ctx.ReadValue<float>();
        if (_moveX < 0 && transform.localScale.x > 0 ||
            _moveX > 0 && transform.localScale.x < 0)
        {
            Flip();
        }

        if (Mathf.Abs(_moveX) > 0)
        {
            _anim.Play("Walk");
            _body.velocity = Vector2.right * transform.localScale.x * walkSpeed;
        }
        else
        {
            _anim.Play("Idle");
            _body.velocity = Vector2.zero;
        }
    }

    private void Flip()
    {
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
