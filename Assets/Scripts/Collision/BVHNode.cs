using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
    AABB nodeAABB;
    List<Body> nodeBodies = new List<Body>();

    BVHNode left;
    BVHNode right;

    public BVHNode(List<Body> bodies)
    {
        nodeBodies = bodies;
        ComputeBoundary();
        Split();
    }

    public void ComputeBoundary()
    {
        if(nodeBodies.Count > 0)
        {
            nodeAABB.center = nodeBodies[0].position;
            nodeAABB.size = Vector3.zero;

            //nodeBodies.ForEach(body => this.nodeAABB.Expand(body.shape.nodeAABB));
        }
    }

    public void Split()
    {
        int length = nodeBodies.Count;
        int half = length / 2;

        if(half >= 1)
        {
            //first half
            //left = new BVHNode(nodeBodies.GetRange());

            //second half
            //right = new BVHNode(nodeBodies.GetRange());

            nodeBodies.Clear();
        }
    }

    public void Query(AABB aabb, List<Body> results)
    {
        if (!nodeAABB.Contains(aabb)) return;

        results.AddRange(nodeBodies);

        left?.Query(aabb, results);
        right?.Query(aabb, results);

    }

    public void Draw()
    {
        nodeAABB.Draw(Color.white);

        left.Draw();
        right.Draw();
    }
}
