using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : BroadPhase
{
    public int capacity { get; set; } = 4;
    QuadTreeNode rootNode;

    public override void Build(AABB aabb, List<Body> bodies)
    {
        queryResultCount = 0;
        // create quadtree root node ??? level
        rootNode = new QuadTreeNode(aabb, capacity, 2);

        // insert bodies starting at root node
        bodies.ForEach(body => rootNode.Insert(body));
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
        //
    }
}
