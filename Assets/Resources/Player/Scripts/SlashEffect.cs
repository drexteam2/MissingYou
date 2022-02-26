using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public int damage;

    public Direction direction;

    private Animator _anim;

    private void Awake()
    {
        _anim ??= GetComponent<Animator>();

        transform.position = PlayerController.Instance.transform.position;
        transform.localScale = PlayerController.Instance.transform.localScale * Mathf.Abs(transform.localScale.x);
    }

    private void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
        transform.localScale = PlayerController.Instance.transform.localScale * Mathf.Abs(transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && direction == Direction.Down)
        {
            PlayerController.Instance.Bounce();
        }
    }

    public void Slash(Direction slashDir)
    {
        direction = slashDir;

        _anim.Play("Slash Effect");
        switch (direction)
        {
            case Direction.Left:
                transform.Rotate(0, 0, -180);
                break;
            case Direction.Right:
                transform.Rotate(0, 0, 180);
                break;
            case Direction.Up:
                transform.Rotate(0, 0, -90 * Mathf.Sign(transform.localScale.x));
                break;
            case Direction.Down:
                transform.Rotate(0, 0, 90 * Mathf.Sign(transform.localScale.x));
                break;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}
