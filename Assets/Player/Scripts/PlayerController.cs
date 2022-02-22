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

    public float walkSpeed;
    public float runSpeed;

    public Interactive interactingWith;

    private Animator _anim;
    private Collider2D _col;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;

    private PlayerInputActions _input;

    private float _moveX;
    
    private bool _interacting;
    private bool _reading;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
        _col ??= GetComponent<Collider2D>();
        _body ??= GetComponent<Rigidbody2D>();
        _sprite ??= GetComponent<SpriteRenderer>();

        _input ??= new PlayerInputActions();
        _instance ??= this;

        UIManager.Instance.GetComponentInChildren<Typewriter>(true).finishedTyping.AddListener(() => _reading = false);
    }

    private void Update()
    {
        _moveX = _input.Player.Movement.ReadValue<float>();
        if (!_interacting && Mathf.Abs(_moveX) > 0)
        {
            if (_moveX < 0 && transform.localScale.x > 0 ||
                _moveX > 0 && transform.localScale.x < 0)
            {
                Flip();
            }
            
            //_anim.Play("Walk");
            _body.velocity = new Vector2(transform.localScale.x * walkSpeed, _body.velocity.y);
        }
        else
        {
            //_anim.Play("Idle");
            _body.velocity = new Vector2(0, _body.velocity.y);
        }
    }

    private void OnEnable()
    {
        StartControl();
    }

    private void OnDisable()
    {
        StopControl();
    }

    public void StopControl()
    {
        _input.Disable();

        _input.Player.Interact.performed -= Interact;
    }

    public void StartControl()
    {
        _input.Enable();

        _input.Player.Interact.performed += Interact;
    }

    private Coroutine _typeRoutine;
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && interactingWith != null)
        {
            var typewriter = UIManager.Instance.GetComponentInChildren<Typewriter>(true);
            if (!_reading && !_interacting)
            {
                UIManager.Instance.ShowUI("Fader");
                _interacting = _reading = true;
                _typeRoutine = StartCoroutine(typewriter.StartTyping(interactingWith.interactText));
            }
            else if (_reading && _interacting)
            {
                _reading = false;
                typewriter.SkipTyping();
                StopCoroutine(_typeRoutine);
            }
            else if (!_reading && _interacting)
            {
                _interacting = false;
                UIManager.Instance.HideUI("Fader");
            }

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
