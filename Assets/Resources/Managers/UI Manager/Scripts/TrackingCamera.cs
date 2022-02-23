using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public GameObject target;
    public float trackingSpeedMultiplier;
    public float maxTrackingSpeed;

    private Rigidbody2D _body;

    private void Awake()
    {
        _body ??= GetComponent<Rigidbody2D>();

        target ??= PlayerController.Instance.gameObject;
    }
    
    private void Update()
    {
        if (target == null) return;

        var distance = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        _body.velocity = distance * trackingSpeedMultiplier;
        _body.velocity = _body.velocity.normalized * Mathf.Clamp(_body.velocity.magnitude, 0, maxTrackingSpeed);
    }
}
