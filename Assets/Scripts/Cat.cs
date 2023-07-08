using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : InteractableObject
{
    private bool isAlive = false;

    public Cat()
    {
        this.element = ElementEnum.Floral;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override ElementEnum OnAbsorption()
    {
        Debug.Log("Absorbed floral.");

        return this.element.Value;
    }

    protected override void OnAssignment(ElementEnum element)
    {
        Debug.Log($"Cat is assigned with {element}");
    }

    public void Revive()
    {
        this.isAlive = true;
    }
}
