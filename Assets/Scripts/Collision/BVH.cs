using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;

        List<Body> sorted = bodies.OrderBy(body => (body.position.x)).ToList();

        rootNode = new BVHNode(sorted);
    }

    public override void Draw()
    {
        rootNode?.Draw();
    }

    public override void Query(AABB aabb, List<Body> bodies)
    {
        rootNode.Query(aabb, bodies);

        queryResultCount += bodies.Count;
    }

    public override void Query(Body body, List<Body> bodies)
    {
        Query(body.shape.GetAABB(body.position), bodies);
    }
}
