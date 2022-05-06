using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collision
{
    public static void CreateBroadPhaseContacts(BroadPhase broadPhase, List<Body> bodies, List<Contact> contacts)
    {
        List<Body> results = new List<Body>();
        for (int i = 0; i < bodies.Count; i++)
        {
            results.Clear();
            // query the broad-phase for potential contacting bodies
            broadPhase.Query(bodies[i], results);

            // add broad-phase contacts 
            for (int j = 0; j < results.Count; j++)
            {
                // check if the result is self and that one of the bodies is a dynamic body
                if (results[j] != bodies[i] &&
                   (results[j].bodyType == Body.eBodyType.DYNAMIC || bodies[i].bodyType == Body.eBodyType.DYNAMIC))
                {
                   // create new contact and add to contacts
                   Contact contact = new Contact() { body1 = bodies[i], body2 = results[j] };
                   contacts.Add(contact);
                    
                }
            }
        }

        //remove dupes in contacts
        contacts.Distinct(new Contact.ItemEqualityComparer());
    }

    public static void CreateNarrowPhaseContacts(List<Contact> contacts)
    {
        // remove contacts from narrow-phase test
        contacts.RemoveAll(contact => (TestOverlap(contact.body1, contact.body2) == false));
        // generate contact info from remaining contacts
        for (int i = 0; i < contacts.Count; i++)
        {
            GenerateContactInfo(contacts[i]);
        }
    }

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
                    contacts.Add(genereateContact(bod1, bod2));
                }

            }
        }
    }

    public static bool TestOverlap(Body bod1, Body bod2)
    {
        return Circle.Intersect(new Circle(bod1), new Circle(bod2));
    }

    public static Contact genereateContact(Body body1, Body body2)
    {
        Contact contact = new Contact();

        contact.body1 = body1;
        contact.body2 = body2;

        Vector2 direction = body1.position - body2.position;
        float distance = direction.magnitude;
        contact.depth = (((CircleShape)body1.shape).radius + ((CircleShape)body2.shape).radius) - distance;

        contact.normal = direction.normalized;

        //Vector2 position = body2.position + (((CircleShape)body2.shape).radius * contact.normal);
        //Debug.DrawRay(position, contact.normal);

        return contact;
    }

    public static void SeparateContacts(List<Contact> contacts)
    {
        foreach(var contact in contacts)
        {
            float totalInverseMass = contact.body1.inverseMass + contact.body2.inverseMass;

            Vector2 separation = contact.normal * (contact.depth / totalInverseMass);
            contact.body1.position += separation * contact.body1.inverseMass;
            contact.body2.position -= separation * contact.body2.inverseMass;
        }
    }

    public static void ApplyImpulses(List<Contact> contacts)
    {
        foreach (var contact in contacts)
        {
            Vector2 relativeVelocity = contact.body1.velocity - contact.body2.velocity;
            float normalVelocity = Vector2.Dot(relativeVelocity, contact.normal);

            if (normalVelocity > 0) continue;

            float totalInverseMass = contact.body1.inverseMass + contact.body2.inverseMass;

            float restitution = (contact.body1.restitution + contact.body2.restitution) * 0.5f;

            float impulseMagnitude = ((1 + restitution) * normalVelocity) / totalInverseMass;

            Vector2 impulse = contact.normal * impulseMagnitude;

            contact.body1.ApplyForce(contact.body1.velocity - (impulse * contact.body1.inverseMass), Body.eForceMode.VELOCITY);
            contact.body2.ApplyForce(contact.body2.velocity + (impulse * contact.body2.inverseMass), Body.eForceMode.VELOCITY);
        }
    }

    public static Contact GenerateContactInfo(Contact contact)
    {
        // compute depth
        Vector2 direction = contact.body1.position - contact.body2.position;
        float distance = direction.magnitude;
        float radius = ((CircleShape)contact.body1.shape).radius + ((CircleShape)contact.body2.shape).radius;
        contact.depth = radius - distance;

        // compute normal
        contact.normal = direction.normalized;

        return contact;
    }
}
