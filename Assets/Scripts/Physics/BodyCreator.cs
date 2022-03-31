using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyCreator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] Body bodyPref;

	bool action = false;
	bool pressed = false;
	float timer = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        action = true;
        pressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        action = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        action = false;
    }

    void Update()
	{
        if(action)
        {
            Vector3 position = Simulator.Instance.GetScreenToWorldPosition(Input.mousePosition);

            Body body = Instantiate(bodyPref, position, Quaternion.identity);
            body.ApplyForce(Random.insideUnitCircle.normalized);

            Simulator.Instance.bodies.Add(body);
        }
	}
}
