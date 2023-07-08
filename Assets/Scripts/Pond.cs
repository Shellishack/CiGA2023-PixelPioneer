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
        Debug.Log("Absorbed water.");
        
        // delete this object
        Destroy(this.gameObject);
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
