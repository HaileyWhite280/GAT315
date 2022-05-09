using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact
{
    public Body body1;
    public Body body2;

    public float depth;
    public Vector2 normal;

    public class ItemEqualityComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact contactA, Contact contactB)
        {
            // Two items are equal if their keys are equal.
            return (contactA.body1 == contactB.body1 && contactA.body2 == contactB.body2) ||
                (contactA.body1 == contactB.body2 && contactA.body2 == contactB.body1);
        }

        public int GetHashCode(Contact obj)
        {
            return obj.GetHashCode();
        }
    }
}
