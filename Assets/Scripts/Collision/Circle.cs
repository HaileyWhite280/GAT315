using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
    public Vector2 center;
    public float radius;

    public Circle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public Circle(Body body)
    {
        this.center = body.position;
        this.radius = ((CircleShape)body.shape).radius;
    }

    public static bool Intersect(Circle circle1, Circle circle2)
    {
        Vector2 direction = circle1.center - circle2.center;
        float distance = direction.magnitude;
        float radius = circle1.radius + circle2.radius;

        return (distance <= radius);
    }
}
