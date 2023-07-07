using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : InteractableObject
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
        Debug.Log("Absorbed floral.");

        // delete this object
        Destroy(this.gameObject);
        return ElementEnum.Floral;
    }

    protected override void OnAssignment(ElementEnum element)
    {
        Debug.Log($"Flower is assigned with {element}");
    }
}
