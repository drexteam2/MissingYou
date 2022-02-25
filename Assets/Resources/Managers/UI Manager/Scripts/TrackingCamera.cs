using System.Collections;
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
    }

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => GameObject.FindGameObjectWithTag("Player") == null);
        
        target = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void Update()
    {
        if (target == null) return;

        var distance = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        _body.velocity = distance * trackingSpeedMultiplier;
        _body.velocity = _body.velocity.normalized * Mathf.Clamp(_body.velocity.magnitude, 0, maxTrackingSpeed);
    }
}
