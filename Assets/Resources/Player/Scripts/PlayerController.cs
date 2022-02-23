using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                var playerObj = Instantiate(Resources.Load<GameObject>("Player/Prefabs/Player"));
                DontDestroyOnLoad(playerObj);
                playerObj.name = playerObj.name.Replace("(Clone)", "");
                playerObj.SetActive(true);
                _instance = playerObj.GetComponent<PlayerController>();
            }

            return _instance;
        }
    }
    
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float jumpBufferTime;
    public float jumpTimeMax;

    public Interactive interactingWith;

    private Animator _anim;
    private Collider2D _col;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;

    private PlayerInputActions _input;

    private float _jumpBufferTimer;
    private float _jumpTime;
    private float _moveX;

    private bool _bufferingJump;
    private bool _jumping;
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
        DontDestroyOnLoad(_instance.gameObject);

        _terrainMask = 1 << LayerMask.NameToLayer("Terrain");

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

        if (_bufferingJump)
        {
            if (IsGrounded())
            {
                _bufferingJump = false;
                _jumpBufferTimer = 0;
                _jumping = true;
            }

            _jumpBufferTimer += Time.deltaTime;
            if (_jumpBufferTimer >= jumpBufferTime)
            {
                _bufferingJump = false;
                _jumpBufferTimer = 0;
            }
        }

        if (_input.Player.Jump.WasPressedThisFrame())
        {
            if (IsGrounded())
            {
                _jumping = true;
            }
            else
            {
                _bufferingJump = true;
            }
        }
        
        if (!_input.Player.Jump.IsPressed() && _jumping)
        {
            _jumping = false;
            _jumpTime = 0;
            if (_body.velocity.y > 0)
            {
                _body.velocity = new Vector2(_body.velocity.x, jumpSpeed / 2);
            }
        }

        if (_jumping)
        {
            //_anim.Play("Jump");
            _jumpTime += Time.deltaTime;
            if (_jumpTime <= jumpTimeMax)
            {
                _body.velocity = new Vector2(_body.velocity.x, jumpSpeed);
            }
            else
            {
                _jumping = false;
                _jumpTime = 0;
                if (_body.velocity.y > 0)
                {
                    _body.velocity = new Vector2(_body.velocity.x, jumpSpeed / 2);
                }
            }
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
            switch (interactingWith.interactionType)
            {
                case InteractionType.Note:
                    var typewriter = UIManager.Instance.GetComponentInChildren<Typewriter>(true);
                    if (!_reading && !_interacting)
                    {
                        UIManager.Instance.ShowUI("Fader");
                        _interacting = _reading = true;
                        _typeRoutine = StartCoroutine(typewriter.StartTyping(((InteractiveNote)interactingWith).interactText));
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
                    break;
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
