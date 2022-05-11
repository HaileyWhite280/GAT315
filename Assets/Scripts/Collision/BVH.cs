using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;
        List<Body> sorted = new List<Body>(bodies);

        //sort bodies along x axis https://stackoverflow.com/questions/24187287/c-sharp-listt-orderby-float-member
        sorted.Sort();

        rootNode = new BVHNode(bodies);
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
        //Query(body.shape.aabb, bodies);
    }
}
