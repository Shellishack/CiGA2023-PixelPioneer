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
		if (isAbsorbed)
		{
			var c = this.GetComponent<SpriteRenderer>().color;
			var cc = GameManager.instance.element.color;
			this.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime / 2);
			GameManager.instance.element.color = new Color(cc.r, cc.g, cc.b, cc.a + Time.deltaTime / 2);
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
	}

	protected override void OnAssignment(ElementEnum element)
	{
		Debug.Log($"Water is assigned with {element}");
		if (element == ElementEnum.Floral)
		{
			this.isPlanted = true;
		}
	}
}