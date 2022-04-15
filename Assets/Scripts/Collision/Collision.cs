using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision
{
    public static void CreateContacts(List<Body> bodies, out List<Contact> contacts)
    {
        contacts = new List<Contact>();

        for(int i = 0; i < bodies.Count - 1; i++)
        {
            for(int j = i + 1; j < bodies.Count; j++)
            {
                Body bod1 = bodies[i];
                Body bod2 = bodies[j];

                if(TestOverlap(bod1, bod2))
                {
                    contacts.Add(new Contact() { body1 = bod1, body2 = bod2 });
                }

            }
        }
    }

    public static bool TestOverlap(Body bod1, Body bod2)
    {
        return Circle.Intersect(new Circle(bod1), new Circle(bod2));
    }
}
