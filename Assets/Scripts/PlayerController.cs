using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1;//0:left 1:middle 2:right
    public float laneDistance = 4;//the distance between two lanes

    public float jumpForce = 100000;
    public float Gravity = -20;
	
	public int health = 3;
	
	bool isJumping = false;
	
    public static PlayerController instance;
	void Awake()
	{
		instance = this;
	}
	
    void Start()
    {
        controller = GetComponent<CharacterController>();
        SwipeController.instance.MoveEvent += MovePlayer;
        SwipeController.instance.ClickEvent += ClickPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        direction.z = forwardSpeed;
		
		if(controller.isGrounded)
        {
            direction.y = -1;
			isJumping = false;
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }
        /*
        


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }*/

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    void MovePlayer(bool[] swipe)
    {
        if (swipe[(int)SwipeController.Direction.Left])
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
        if (swipe[(int)SwipeController.Direction.Right])
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
		if (swipe[(int)SwipeController.Direction.Up] && !isJumping )
        {
			jump();
			isJumping = true;
        }
    }
	
	void ClickPlayer( bool Click )
	{
		if ( Click && !isJumping )
		{
			jump();
			isJumping = true;
		}
	}
	
	public void takeDamage()
	{
		if (health > 1)
		{
			health--;
		}
		else
		{
			health = 3;
			General.instance.ResetLevel();
		}
	}

    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void jump()
    {
        direction.y = jumpForce;
		
    }
	
}
