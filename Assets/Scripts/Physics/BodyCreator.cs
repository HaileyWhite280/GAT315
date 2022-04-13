using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyCreator : MonoBehaviour
{
    [SerializeField] Body bodyPref;
    [SerializeField] FloatData speed;
    [SerializeField] FloatData size;
    [SerializeField] FloatData density;
    [SerializeField] FloatData drag;
    [SerializeField] EnumData bodyType;

	bool action = false;
	bool pressed = false;
	//float timer = 0;

    public void OnPointerDown()
    {
        if(Input.GetMouseButton(0))
        {
            action = true;
            pressed = true;

        }
    }

    public void OnPointerExit()
    {
        action = false;
    }

    public void OnPointerUp()
    {
        action = false;
    }

    void Update()
	{
        if(action && (pressed || Input.GetKey(KeyCode.LeftControl)))
        {
            pressed = false;

            Vector3 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);

            Body body = Instantiate(bodyPref, position, Quaternion.identity);

            body.bodyType = (Body.eBodyType)bodyType.value;
            body.shape.size = size.value;
            body.shape.density = density.value;
            body.drag = drag.value;

            body.ApplyForce(Random.insideUnitCircle.normalized * speed.value, Body.eForceMode.VELOCITY);

            Simulator.Instance.bodies.Add(body);
        }
	}
}
