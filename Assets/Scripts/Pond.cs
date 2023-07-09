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

	private void Update()
	{
		if (isAbsorbed)
		{
			var c = this.GetComponent<SpriteRenderer>().color;
			this.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a - 0.001f);
			if (c.a <= 0)
			{
				GameManager.instance.player.GetComponent<PlayerController>().finishInteraction = true;
				GameManager.instance.ShowNormalDialogue("这是……我的力量？", 0.05f);
				Destroy(this.gameObject);
			}
		}
	}

	protected override ElementEnum OnAbsorption()
	{
		Debug.Log("Absorbed water.");
		isAbsorbed = true;
		// Change to sprite renderer color to #8E1E15
		//this.GetComponent<SpriteRenderer>().color = new Color(0.5568628f, 0.1176471f, 0.08235294f, 1f);
		return this.element.Value;
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