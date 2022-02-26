using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEmitter : MonoBehaviour
{
    public IEnumerator SpewBlood(Direction direction)
    {
        var ps = GetComponent<ParticleSystem>();
        ParticleSystem.VelocityOverLifetimeModule vel = ps.velocityOverLifetime;
        switch (direction)
        {
            case Direction.Left:
                vel.x = -5;
                vel.y = 0;
                break;
            case Direction.Right:
                vel.x = 5;
                vel.y = 0;
                break;
            case Direction.Up:
                vel.x = 0;
                vel.y = 5;
                break;
            case Direction.Down:
                vel.x = 0;
                vel.y = -5;
                break;
        }

        ps.Emit(50);

        yield return new WaitForSeconds(ps.main.duration);

        Destroy(gameObject);
    }
}
