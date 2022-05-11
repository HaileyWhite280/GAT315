using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] List<Force> forces;
	[SerializeField] StringData fps;
	[SerializeField] IntData fixedFPS;
	[SerializeField] BoolData simulate;
	[SerializeField] StringData collisionInfo;
	//[SerializeField] List<Force> force;
	
	BroadPhase broadPhase = new BVH();

	public List<Body> bodies { get; set; } = new List<Body>();
	Camera activeCamera;

	private float timeAccumulator = 0;
	public float fixedDeltaTime => 1.0f / fixedFPS.value;

/*	BroadPhase[] broadPhases { new QuadTree(), new BVH(), new NullBroadPhase() };
	BroadPhase broadphase;*/

	private void Start()
	{
		activeCamera = Camera.main;
	}

	public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}

    private void Update()
    {
		fps.value = (1.0f / Time.deltaTime).ToString("F2");

		if (!simulate.value) return;

		timeAccumulator += Time.deltaTime;

		forces.ForEach(force => force.ApplyForce(bodies));

		Vector2 screenSize = GetScreenSize();

		/*broadPhase = broadPhases[broadPhaseType.value];*/

		while(timeAccumulator >= fixedDeltaTime)
        {
			broadPhase.Build(new AABB(Vector2.zero, screenSize), bodies);

			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);
			Collision.CreateNarrowPhaseContacts(contacts);

			Collision.SeparateContacts(contacts);
			Collision.ApplyImpulses(contacts);
			
			bodies.ForEach(body => { 
				Integrator.SemiImplicitEuler(body, timeAccumulator); 
				body.position = body.position.Wrap(-GetScreenSize() / 2, GetScreenSize() / 2);
			});

			timeAccumulator -= fixedDeltaTime;
        }

		broadPhase.Draw();
		collisionInfo.value = broadPhase.queryResultCount + "/" + bodies.Count;

/*		bodies.ForEach(body =>
		{
			//body.Step(Time.deltaTime);
			Integrator.SemiImplicitEuler(body, Time.deltaTime);
		});*/

		bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public Body GetScreenToBody(Vector3 screen)
    {
		Body body = null;
		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

		if(hit.collider)
        {
			hit.collider.gameObject.TryGetComponent<Body>(out body);
        }

		return body;
    }

	public Vector2 GetScreenSize()
    {
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
    }

	public void Clear()
    {
		bodies.ForEach(body => Destroy(body.gameObject));
		bodies.Clear();
    }
}
