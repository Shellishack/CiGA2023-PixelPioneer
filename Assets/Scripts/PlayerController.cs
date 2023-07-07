using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Collider2D feetChecker;
	private Rigidbody2D rdBody;

	[Header("�������")]
	public float moveSpeed;

	public float jumpStrength;

	[Header("���״̬")]
	public bool moveState = false;

	//���� -1:Left 1:right
	public int faceDirction = 1;

	//��Ծ״̬
	public bool jumpState = false;

	//�Ƿ������Ծ
	public bool isCanjump = true;

	//�Ƿ��ڵ���
	public bool isOnGround = true;

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

	//�����������
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
		if (Input.GetKey(KeyCode.W))
		{
			jumpState = false;
			if (isCanjump && isOnGround)
			{
				jumpState = true;
			}
		}
	}

	private void ProcessMovement()
	{
		rdBody.velocity = new Vector2(moveSpeed * faceDirction * Time.fixedDeltaTime * (moveState ? 1 : 0), rdBody.velocity.y);
		if (jumpState)
		{
			rdBody.velocity = new Vector2(rdBody.velocity.x, jumpStrength * Time.fixedDeltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			isOnGround = true;
			Debug.Log("OnGround");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			isOnGround = false;
			Debug.Log("ExitGround");
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		//Gizmos.DrawWireSphere(transform.position - new Vector3(0f, 0.45f, 0f), 0.4f);
	}
}