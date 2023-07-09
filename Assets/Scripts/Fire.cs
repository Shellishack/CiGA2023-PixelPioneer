using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : InteractableObject
{
	public bool deleteState = false;

	public Fire()
	{
		this.element = ElementEnum.Fire;
	}

	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
		if (deleteState)
		{
			if (!isAbsorbed)
			{
				GameManager.instance.element.GetComponent<Element>().target = this.transform;
				if (GameManager.instance.element.GetComponent<Element>().arrive)
				{
					var c = GetComponent<SpriteRenderer>().color;
					var cc = GameManager.instance.element.color;
					GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime / 3);
					GameManager.instance.element.color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime / 3);
					GameManager.instance.fireBg.volume -= Time.deltaTime / 6;
					if (c.a <= 0)
					{
						GameManager.instance.element.GetComponent<Element>().target = GameManager.instance.player.transform.GetChild(0).transform;
						GameManager.instance.ShowSelfTalk("�Բ���������һ���û�ܸı����ͺ������");
						GameManager.instance.ShowSelfTalk("w3=�����ȥ���ɻ���һ�����̴�Զ������");
						GameManager.instance.fireBg.Pause();
						Destroy(gameObject);
					}
				}
			}
			else
			{
				GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = false;
				GameManager.instance.element.sprite = GameManager.instance.fireElement;
				var c = GetComponent<SpriteRenderer>().color;
				var cc = GameManager.instance.element.color;
				GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a - Time.deltaTime / 3);
				GameManager.instance.element.color = new Color(cc.r, cc.g, cc.b, cc.a + Time.deltaTime / 3);
				if (c.a <= 0)
				{
					GameManager.instance.player.GetComponent<PlayerController>().isAllowPlayerAction = true;
					GameManager.instance.fireExit.Play();
					GameManager.instance.player.GetComponent<PlayerController>().finishInteraction = true;
					GameManager.instance.player.GetComponent<PlayerController>().begingInteraction = false;
					GameManager.instance.ShowSelfTalk("����������Ϩ���ˣ������Ҹղ���ô��ȡ�������棿");
					GameManager.instance.ShowSelfTalk("w3=Ҳ����������ȡ�Ĺؼ���");
					Destroy(gameObject);
				}
			}
		}
	}

	protected override ElementEnum OnAbsorption()
	{
		Debug.Log("Absorbed fire.");
		// delete this object
		if (isAbsorbed)
		{
			deleteState = true;
			//Destroy(this.gameObject);
		}
		return this.element.Value;
	}

	protected override void OnAssignment(ElementEnum element)
	{
		if (deleteState) return;
		Debug.Log($"Fire is assigned with {element}");
		if (element == ElementEnum.Water)
		{
			deleteState = true;
			// delete this if it is assigned with water
			//Destroy(this.gameObject);
		}
	}
}