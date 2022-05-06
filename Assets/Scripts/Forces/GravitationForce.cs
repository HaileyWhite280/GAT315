using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationForce : Force
{
    [SerializeField] FloatData gravitation;

    public override void ApplyForce(List<Body> bodies)
    {
        if(gravitation.value == 0)
        {
            return;
        }

        for(int i = 0; i < bodies.Count; i++)
        {
            for(int j = 0; j < bodies.Count; j++)
            {
                if (i == j) continue;

                Body body1 = bodies[i];
                Body body2 = bodies[j];

                Vector2 direction = body1.position - body2.position;
                float distanceSqr = Mathf.Max(1, direction.sqrMagnitude);
                float force = gravitation.value * (body1.mass * body2.mass) / distanceSqr;

                body1.ApplyForce(-direction.normalized * force, Body.eForceMode.FORCE);
                body2.ApplyForce(direction.normalized * force, Body.eForceMode.FORCE);
            }
        }
    }
}
