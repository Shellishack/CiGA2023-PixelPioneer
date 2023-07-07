using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : InteractableObject
{
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
        return ElementEnum.Water;
    }

    protected override void OnAssignment(ElementEnum element)
    {
        Debug.Log($"Water is assigned with {element}");
    }
}
