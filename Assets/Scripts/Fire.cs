using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : InteractableObject
{
    public Fire()
    {
        this.element = ElementEnum.Fire;
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
        Debug.Log("Absorbed fire.");

        // delete this object
        Destroy(this.gameObject);
        return this.element.Value;
    }

    protected override void OnAssignment(ref ElementEnum? element)
    {
        Debug.Log($"Fire is assigned with {element}");
        if (element == ElementEnum.Water)
        {
            element = null;

            // delete this if it is assigned with water
            Destroy(this.gameObject);
        }
    }
}
