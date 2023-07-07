using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Collider2D feetChecker;
    private Rigidbody2D rdBody;

    [Header("玩家属性")]
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
    private InteractableObject interactableObject = null;

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
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    //处理玩家输入
    private void ProcessInput()
    {
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
            if (interactableObject != null && absorbedElement == null)
            {
                absorbedElement = interactableObject.Absorb();
            }
        }
        // 使用元素
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (interactableObject != null && absorbedElement != null)
            {
                interactableObject.Assign(absorbedElement.Value);
                absorbedElement = null;
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
}