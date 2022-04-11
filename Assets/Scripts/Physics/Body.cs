using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public enum eForceMode
    {
        FORCE,
        VELOCITY,
        ACCELERATION
    }

    public enum eBodyType
    {
        STATIC,
        KINEMATIC,
        DYNAMIC
    }

    [Tooltip("Shape of Body")]
    public Shape shape;
    public List<Spring> springs { get; set; } = new List<Spring>();

    public eBodyType bodyType { get; set; } = eBodyType.DYNAMIC;

    public Vector2 position { get => transform.position; set => transform.position = value; }
    public Vector2 velocity { get; set; } = Vector2.zero;
    public Vector2 acceleration { get; set; } = Vector2.zero;
    //public Vector2 force { get; set; } = Vector2.zero;

    public float drag { get; set; } = 0;
    public float mass => shape.mass;
    public float inverseMass { get => (mass == 0 || bodyType != eBodyType.DYNAMIC) ? 0 : 1 / mass; }
    
    public void ApplyForce(Vector2 force, eForceMode forceMode)
    {
        if (bodyType != eBodyType.DYNAMIC) return;

        switch (forceMode)
        {
            case eForceMode.FORCE:
                acceleration += force * inverseMass;
                break;
            case eForceMode.ACCELERATION:
                acceleration += force;
                break;
            case eForceMode.VELOCITY:
                velocity = force;
                break;
            default:
                break;
        }

        //this.force += force;
    }

    public void Step(float dt)
    {
        //acceleration = Simulator.Instance.gravity + (force * inverseMass);
    }
}
