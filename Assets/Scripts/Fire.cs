using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : InteractableObject
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
        Debug.Log("Absorbed fire.");

        // delete this object
        Destroy(this.gameObject);
        return ElementEnum.Fire;
    }

    protected override void OnAssignment(ElementEnum element)
    {
        Debug.Log($"Fire is assigned with {element}");
    }
}
