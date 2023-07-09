using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : InteractableObject
{
	public bool isPlanted = false;


	public Pond()
	{
		this.element = ElementEnum.Water;
	}

	private void Update()
	{
		if (isPlanted)
		{
            this.transform.Find("pond_pure").gameObject.SetActive(false);
            this.transform.Find("pond_planted").gameObject.SetActive(true);
        }
		else
		{
            this.transform.Find("pond_pure").gameObject.SetActive(true);
            this.transform.Find("pond_planted").gameObject.SetActive(false);
        }
	}

	protected override ElementEnum OnAbsorption()
	{
		Debug.Log("Absorbed water.");

		// Change to sprite renderer color to #8E1E15
		this.GetComponent<SpriteRenderer>().color = new Color(0.5568628f, 0.1176471f, 0.08235294f, 1f);
		this.isAbsorbed = true;

        return this.element.Value;
	}

	protected override void OnAssignment(ref ElementEnum? element)
	{
		Debug.Log($"Water is assigned with {element}");
		if (element == ElementEnum.Floral)
		{
			this.isPlanted = true;
			this.isAssignedElement = true;
			element = null;
		}
	}
}