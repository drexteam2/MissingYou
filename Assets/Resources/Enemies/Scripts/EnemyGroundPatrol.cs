using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//written by Gabe

public class EnemyGroundPatrol : MonoBehaviour
{
    //variables and such
    public bool patrol;
    public float patrolSpeed;
    public float patrolTimer;

    private bool mustFlip;
    private float countdown;

    public Rigidbody2D rigidBody;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    //public Collider2D bodyCollider;

    // Start is called before the first frame update
    void Start()
    {
        patrol = true ;

        countdown = patrolTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //if patrol is true, goes on patrol (can be set to false for debugging)
        if (patrol == true)
        {
            groundPatrol();
        }

        countdown -= Time.deltaTime;

        //if countdown = 0, flips sprite in other direction
        if (countdown<=0){
            //mustFlip = true if circle is in the ground, false if not
            mustFlip = !mustFlip;
        }

        //resets timer when it hits 0 and after sprite flips
        if (countdown <= 0)
        {
            countdown = patrolTimer;
        }
    }

    private void FixedUpdate()
    {
        //add bodyCollider.IsTouchingLayers(groundLayer) to conditional later, use ||
        if (patrol == true){
            //mustFlip = true if circle is in the ground, false if not
            mustFlip = !(Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer));
        }
    }

    //Enemy moves in a straight line until x around of time, it hits the edge of a platform, or it hits a wall
    private void groundPatrol()
    {
        //if enemy reaches an edge, it turns around
        if (mustFlip == true)
        {
            Flip();
        }

        //sets velocity. Starts negative so enemy goes left on startup
        rigidBody.velocity = new Vector2(-patrolSpeed * Time.fixedDeltaTime, rigidBody.velocity.y);
    }

    //Flips the sprite and sets the walk speed in the opposite direction
    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        patrolSpeed *= -1;
    }
}
