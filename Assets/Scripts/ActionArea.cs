using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionArea : InteractableObject
{
	protected override ElementEnum OnAbsorption()
	{
		return ElementEnum.Fire;
	}

	protected override void OnAssignment(ref ElementEnum? element)
	{
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