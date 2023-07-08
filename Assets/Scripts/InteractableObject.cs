using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
	// 是否已经被吸收
	private bool isAbsorbed = false;

	// 是否已经被赋予元素
	private bool isAssignedElement = false;

	public bool canAbsorb = true;
	public bool canAssign = true;

	public bool isHighlighted = false;

	public ElementEnum? element = null;

	//可交互物品的描述
	public bool showDescription = true;

	public InteractionObjectDescriptionsSO descriptions;

	protected abstract ElementEnum OnAbsorption();

	protected abstract void OnAssignment(ElementEnum element);

	public ElementEnum? Absorb()
	{
		if (!canAbsorb) return null;
		this.isAbsorbed = true;
		return this.OnAbsorption();
	}

	public void Assign(ElementEnum element)
	{
		if (!canAssign) return;
		this.isAssignedElement = true;
		this.OnAssignment(element);
	}
}