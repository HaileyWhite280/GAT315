using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{
    public Body body1 { get; set; }
    public Body body2 { get; set; }

    public float restLength { get; set; }
    public float k { get; set; }

    public void AppyForce()
    {
        Vector2 direction = body1.position - body2.position;

        float length = direction.magnitude;
        float x = length - restLength;

        float f = -k * x;

        body1.ApplyForce(f * direction.normalized, Body.eForceMode.FORCE);
        body2.ApplyForce(-f * direction.normalized, Body.eForceMode.FORCE);

        Debug.DrawLine(body1.position, body2.position);
    }

    static public Vector2 Force(Vector2 pos1, Vector2 pos2, float restLength, float k)
    {
        Vector2 direction = pos1 - pos2;

        float length = direction.magnitude;
        float x = length - restLength;

        float f = -k * x;

        return f * direction.normalized;
    }
}
