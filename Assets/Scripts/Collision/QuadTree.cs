using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : BroadPhase
{
    QuadTreeNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        throw new System.NotImplementedException();
    }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override void Query(AABB aabb, List<Body> bodies)
    {
        throw new System.NotImplementedException();

        rootNode.Query(aabb, bodies);
    }

    public override void Query(Body body, List<Body> bodies)
    {
        throw new System.NotImplementedException();
    }
}
