using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    // 是否已经被吸收
    private bool isAbsorbed = false;
    // 是否已经被赋予元素
    private bool isAssignedElement = false;

    protected abstract ElementEnum OnAbsorption();

    protected abstract void OnAssignment(ElementEnum element);

    public ElementEnum Absorb()
    {
        this.isAbsorbed = true;
        return this.OnAbsorption();
    }

    public void Assign(ElementEnum element)
    {
        this.isAssignedElement = true;
        this.OnAssignment(element);
    }
}
