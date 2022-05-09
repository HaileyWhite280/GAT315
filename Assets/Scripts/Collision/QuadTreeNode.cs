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
/*        
        // insert body into the newly subdivided nodes
        northeast.Insert(body);
        // insert northwest...*/

        if (!nodeAABB.Contains(body.shape.GetAABB(body.position)))
        {
            if(nodeBodies.Count < nodeCapacity)
            {
                nodeBodies.Add(body);
            }
            else
            {
                if(!subdivided)
                {
                    Subdivide();
                }

                // ???
                NE.Insert(body);
                NW.Insert(body);
                SE.Insert(body);
                SW.Insert(body);
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

    public void Draw()
    {
        /*        nodeAABB.Draw(Color.green);

                NE.Draw();
                NW.Draw();
                SE.Draw();
                SW.Draw(); ???*/

        Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];

        nodeAABB.Draw(color);
        nodeBodies.ForEach(body => Debug.DrawLine(nodeAABB.center, body.position, color));
    }

    private void Subdivide()
    {
        float xo = nodeAABB.extents.x * 0.5f;
        float yo = nodeAABB.extents.y * 0.5f;

        NE = new QuadTreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        NW = new QuadTreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        SE = new QuadTreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);
        SW = new QuadTreeNode(new AABB(new Vector2(nodeAABB.center.x - xo, nodeAABB.center.y + yo), nodeAABB.extents), nodeCapacity, nodeLevel + 1);

        subdivided = true;
    }
}
