using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private Animator _anim;
    private Collider2D _col;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;

    private PlayerInputActions _input;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();
        _col ??= GetComponent<Collider2D>();
        _body ??= GetComponent<Rigidbody2D>();
        _sprite ??= GetComponent<SpriteRenderer>();

        _input = new PlayerInputActions();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
