using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    AABB nodeAABB;
    int nodeCapacity;
    int nodeLevel;
    List<Body> nodeBodies = new List<Body>();

    QuadTreeNode NE;
    QuadTreeNode NW;
    QuadTreeNode SE;
    QuadTreeNode SW;

    bool subdivided = false;

    public QuadTreeNode(AABB aabb, int capacity, int level)
    {
        nodeAABB = aabb;
        nodeCapacity = capacity;
        nodeLevel = level;
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

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        results.AddRange(nodeBodies);

        if(subdivided)
        {
            NE.Query(aabb, results);
            NW.Query(aabb, results);
            SE.Query(aabb, results);
            SW.Query(aabb, results);
        }
    }

    //subdivide add nodeLevel + 1 to end after nodeCapacity
    //draw         Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];
/*    nodeAABB.Draw(color);
        nodeBodies.ForEach(body => Debug.DrawLine(nodeAABB.center, body.position, color));*/
}
