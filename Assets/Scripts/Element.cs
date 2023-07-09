using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
	public Transform target;
	public float speed;
	public bool arrive;

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
		arrive = Vector3.Distance(transform.position, target.position) < 0.01;
	}
}