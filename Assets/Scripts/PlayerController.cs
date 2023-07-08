using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rdBody;

	[Header("玩家属性")]
	public bool isAllowPlayerAction;//是否允许玩家操作

	public float moveSpeed;

	public float jumpStrength;

	public bool isDoubleJump = false;

	private bool doubleJumpState = false;

	[Header("玩家状态")]
	public bool moveState = false;

	//朝向 -1:Left 1:right
	public int faceDirction = 1;

	//跳跃状态
	public bool jumpState = false;

	//是否可以跳跃
	public bool isCanjump = true;

	//是否在地面
	public bool isOnGround = true;

	// 周围可互动物体
	private InteractableObject iObject;

	public InteractableObject interactableObject
	{
		get { return iObject; }
		set
		{
			GameManager.instance.currentInteractableObject = value;
			iObject = value;
		}
	}

	// 玩家拥有的元素
	private ElementEnum? absorbedElement = null;

	// Start is called before the first frame update
	private void Start()
	{
		rdBody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		ProcessInput();
		//Vector3 canvas =
		transform.localScale = new Vector3(faceDirction, 1);
		transform.GetChild(0).localScale = new Vector3(faceDirction, 1);
	}

	private void FixedUpdate()
	{
		ProcessMovement();
	}

	//处理玩家输入
	private void ProcessInput()
	{
		if (!isAllowPlayerAction) return;
		moveState = true;
		//left
		if (Input.GetKey(KeyCode.A))
		{
			faceDirction = -1;
		}
		//right
		else if (Input.GetKey(KeyCode.D))
		{
			faceDirction = 1;
		}
		else
		{
			moveState = false;
		}
		//up
		if (Input.GetKeyDown(KeyCode.W))
		{
			Debug.Log("Jump!");
			if (isCanjump && isOnGround)
			{
				jumpState = true;
			}
			else if (isDoubleJump && isCanjump && !isOnGround && !doubleJumpState)
			{
				jumpState = true;
				doubleJumpState = true;
			}
		}

		// 吸收元素
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (!absorbedElement.HasValue)
			{
				if (interactableObject != null)
				{
					absorbedElement = interactableObject.Absorb();
				}
			}
			// Craft element if water and fire are present
			else
			{
				var objElement = interactableObject.element;
				if (objElement.HasValue && objElement.Value == ElementEnum.Water && absorbedElement.Value == ElementEnum.Fire)
				{
					absorbedElement = ElementEnum.Floral;
					Destroy(interactableObject.gameObject);
					interactableObject = null;
					Debug.Log("Crafted floral element!");
				}
				else if (objElement.HasValue && objElement.Value == ElementEnum.Fire && absorbedElement.Value == ElementEnum.Water)
				{
					absorbedElement = ElementEnum.Floral;
					Destroy(interactableObject.gameObject);
					interactableObject = null;
					Debug.Log("Crafted floral element!");
				}
				else
				{
					Debug.Log("Cannot craft element");
				}
			}
		}
		// 使用元素
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			if (interactableObject != null && absorbedElement.HasValue)
			{
				interactableObject.Assign(ref absorbedElement);
			}
		}
	}

	private void ProcessMovement()
	{
		rdBody.velocity = new Vector2(moveSpeed * faceDirction * Time.fixedDeltaTime * (moveState ? 1 : 0), rdBody.velocity.y);
		if (jumpState)
		{
			rdBody.velocity = new Vector2(rdBody.velocity.x, jumpStrength * Time.fixedDeltaTime);
			jumpState = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			isOnGround = true;
			doubleJumpState = false;
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
		{
			if (interactableObject == null)
			{
				Debug.Log($"Enter {collision.gameObject.name}!");
				interactableObject = collision.gameObject.GetComponent<InteractableObject>();
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			isOnGround = true;
			doubleJumpState = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			isOnGround = false;
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
		{
			Debug.Log($"Leave {collision.gameObject.name}!");
			interactableObject = null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		//Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 0.45f, 0f), 0.4f);
	}

	//-----------------控制开场动画相关---------------------
	public void Show_Dialogue_1()
	{
		GameManager.instance.ShowNormalDialogue("刚刚的蝴蝶去哪了?我怎么跑到这里来了?", 0.05f);
	}

	public void Show_Dialogue_2()
	{
		GameManager.instance.ShowNormalDialogue("……找不到回去的路了", 0.05f, 1.5f);
	}

	public void Show_Dialogue_3()
	{
		GameManager.instance.ShowNormalDialogue("算了，还好我经常来这个森林，我记得还有其他的路，总之先找找看吧", 0.05f, 1.5f);
	}

	public void Show_SceneName()
	{
		GameManager.instance.ShowSceneName("-森林外-");
	}
}