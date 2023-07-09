using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
	// 是否已经被吸收
	protected bool isAbsorbed = false;

	// 是否已经被赋予元素
	protected bool isAssignedElement = false;

	public bool canAbsorb = true;
	public bool canAssign = true;

	public ElementEnum? element = null;

	//可交互物品的描述
	public bool showDescription = true;

	public InteractionObjectDescriptionsSO descriptions;

	protected abstract ElementEnum OnAbsorption();

	protected abstract void OnAssignment(ref ElementEnum? element);

    protected virtual void OnTriggerEnter2D_Child(Collider2D collision) { }

    protected virtual void OnTriggerExit2D_Child(Collider2D collision) { }


    public virtual ElementEnum? Absorb()
	{
		if (!canAbsorb || isAbsorbed || isAssignedElement) return null;
        this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
        this.transform.Find("E_hint").gameObject.SetActive(false);
        this.transform.Find("Q_hint").gameObject.SetActive(false);
        return this.OnAbsorption();
	}

	public virtual void Assign(ref ElementEnum? element)
	{
		if (!canAssign || isAbsorbed || isAssignedElement || element == this.element) return;
		this.OnAssignment(ref element);
        this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
        this.transform.Find("E_hint").gameObject.SetActive(false);
        this.transform.Find("Q_hint").gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAbsorbed || isAssignedElement)
        {
            this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
            this.transform.Find("E_hint").gameObject.SetActive(false);
            this.transform.Find("Q_hint").gameObject.SetActive(false);
        }
        else
        {
            if (canAbsorb && canAssign)
            {
                // Find and activate the canvas named "E_or_Q_hint" in the current game object's hierarchy
                this.transform.Find("E_or_Q_hint").gameObject.SetActive(true);
                this.transform.Find("E_hint").gameObject.SetActive(false);
                this.transform.Find("Q_hint").gameObject.SetActive(false);
            }
            else if (canAbsorb)
            {
                this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
                this.transform.Find("E_hint").gameObject.SetActive(true);
                this.transform.Find("Q_hint").gameObject.SetActive(false);
            }
            else if (canAssign)
            {
                this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
                this.transform.Find("E_hint").gameObject.SetActive(false);
                this.transform.Find("Q_hint").gameObject.SetActive(true);
            }
            else
            {
                this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
                this.transform.Find("E_hint").gameObject.SetActive(false);
                this.transform.Find("Q_hint").gameObject.SetActive(false);
            }
        }

        OnTriggerEnter2D_Child(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        this.transform.Find("E_or_Q_hint").gameObject.SetActive(false);
        this.transform.Find("E_hint").gameObject.SetActive(false);
        this.transform.Find("Q_hint").gameObject.SetActive(false);

        OnTriggerExit2D_Child(collision);
    }
}