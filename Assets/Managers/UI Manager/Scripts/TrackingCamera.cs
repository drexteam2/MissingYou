using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public GameObject target;
    public float trackingForce;
    public float lockOnThreshold;
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
        if (distance.magnitude < lockOnThreshold)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            _body.velocity = Vector2.zero;
        }
        else
        {
            _body.AddForce(distance * trackingForce);
            _body.velocity = _body.velocity.normalized * Mathf.Clamp(_body.velocity.magnitude, 0, maxTrackingSpeed);
        }
    }
}
