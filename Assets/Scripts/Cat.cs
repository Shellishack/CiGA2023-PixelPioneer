using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : InteractableObject
{
	public bool isAlive = false;

	public bool isJump = false;

	public Sprite deadStateSprite;
	public Sprite standStateSprite;
	public Sprite jumpStateSprite;

	public Cat()
	{
		this.element = ElementEnum.Floral;
	}

	// Start is called before the first frame update
	private void Start()
	{
    }

	// Update is called once per frame
	private void Update()
	{
        // If the cat is dead, change the sprite to dead cat
        if (!this.isAlive)
        {
            this.GetComponent<SpriteRenderer>().sprite = deadStateSprite;
        }
        else if (isJump)
        {
            this.GetComponent<SpriteRenderer>().sprite = jumpStateSprite;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = standStateSprite;
        }
    }

    public override ElementEnum? Absorb()
    {
        if (!canAbsorb) return null;
        return this.OnAbsorption();
    }

    public override void Assign(ref ElementEnum? element)
    {
        if (!canAssign || isAssignedElement) return;
        this.OnAssignment(ref element);
    }


    protected override ElementEnum OnAbsorption()
	{
		Debug.Log("Absorbed floral.");

		return this.element.Value;
	}

	protected override void OnAssignment(ref ElementEnum? element)
	{
		Debug.Log($"Cat is assigned with {element}");
		if (element == ElementEnum.Floral)
		{
			element = null;
			this.Revive();
		}
	}

	public void Revive()
	{
		this.isAlive = true;
	}


    protected override void OnTriggerEnter2D_Child(Collider2D collision)
    {
		// If the cat is alive and collides with the player
		var layer = LayerMask.NameToLayer("Player");
        if (this.isAlive && collision.gameObject.layer == layer)
		{
			PlaySound();
		}
    }
    private void PlaySound()
	{
		// Play cat sound
		this.gameObject.GetComponent<AudioSource>().Play();
	}
}