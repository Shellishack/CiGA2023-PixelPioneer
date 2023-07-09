using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionArea : InteractableObject
{
	public bool touchDead = false;

	protected override ElementEnum OnAbsorption()
	{
		return ElementEnum.Fire;
	}

	protected override void OnAssignment(ElementEnum element)
	{
		Debug.Log("asdasd");
	}

	// Start is called before the first frame update
	private void Start()
	{
		canAbsorb = false;
	}

	// Update is called once per frame
	private void Update()
	{
	}
}