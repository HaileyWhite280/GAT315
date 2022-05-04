using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    AABB nodeAABB;
    int nodeCapacity;
    List<Body> nodeBodies = new List<Body>();

    QuadTreeNode NE;
    QuadTreeNode NW;
    QuadTreeNode SE;
    QuadTreeNode SW;

    bool subdivided = false;

    public QuadTreeNode(AABB aabb, int capacity)
    {
        nodeAABB = aabb;
        nodeCapacity = capacity;
    }

    public void Insert(Body body)
    {
        if (!nodeAABB.Contains(body.shape.GetAABB(body.position)))
        {
            if(nodeBodies.Count < nodeCapacity)
            {
                nodeBodies.Add(body);
            }
            else
            {
                //subdivide
            }
        }
    }

}
