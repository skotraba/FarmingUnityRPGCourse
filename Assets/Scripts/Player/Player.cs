using UnityEngine;

public class Player : SingletonMonoBehavior<Player>
{
  //Movement Parameters
  public float xInput; 
  public float yInput;
  public bool isWalking; 
  public bool isRunning; 
  public bool isIdle;
  public bool isCarrying; 
  public ToolEffect toolEffect;
  public bool isUsingToolRight; 
  public bool isUsingToolLeft;
  public bool isUsingToolUp;
  public bool isUsingToolDown;
  public bool isLiftingToolRight; 
  public bool isLiftingToolLeft; 
  public bool isLiftingToolUp; 
  public bool isLiftingToolDown;
  public bool isPickingRight; 
  public bool isPickingLeft; 
  public bool isPickingUp; 
  public bool isPickingDown;
  public bool isSwingingToolRight; 
  public bool isSwingingToolLeft; 
  public bool isSwingingToolUp; 
  public bool isSwingingToolDown;
  public bool idleUp; 
  public bool idleDown; 
  public bool idleLeft; 
  public bool idleRight;

  private Camera mainCamera;

  private Rigidbody2D rigidBody2D;

  public Direction playerDirection;

  public float movementSpeed;

  private bool _playerinputIsDisabled = false;

  public bool PlayerInputIsDisabled { get => _playerinputIsDisabled; set => _playerinputIsDisabled = value; }

  protected override void Awake()
  {
    base.Awake();

    rigidBody2D = GetComponent<Rigidbody2D>();

    // Get reference to camera
    mainCamera = Camera.main;
  }

  private void Update()
  {
    #region Player Input

    ResetAnimationTriggers();

    PlayerMovementInput();

    PlayerWalkInput();

    EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
      isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
      isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
      isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
      isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
      idleUp, idleDown, idleLeft, idleRight);

    #endregion
  }

  private void FixedUpdate()
  {
    PlayerMovement();
  }

  private void PlayerMovement()
  {
    Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime);
    rigidBody2D.MovePosition(rigidBody2D.position + move);
  }

  private void ResetAnimationTriggers()
  {
    isUsingToolRight = false; 
    isUsingToolLeft = false;
    isUsingToolUp = false;
    isUsingToolDown = false;
    isLiftingToolRight = false;
    isLiftingToolLeft = false; 
    isLiftingToolUp = false; 
    isLiftingToolDown = false;
    isPickingRight = false; 
    isPickingLeft = false; 
    isPickingUp = false; 
    isPickingDown = false;
    isSwingingToolRight = false;
    isSwingingToolLeft = false;
    isSwingingToolUp = false; 
    isSwingingToolDown = false;
    toolEffect = ToolEffect.none;
  }

  private void PlayerMovementInput()
  {
    yInput = Input.GetAxisRaw("Vertical");
    xInput = Input.GetAxisRaw("Horizontal");

    if(yInput != 0 && xInput != 0)
    {
      xInput = xInput * 0.71f;
      yInput = yInput * 0.71f;
    }

    if (xInput != 0 || yInput != 0)
    {
      isRunning = true;
      isWalking = false;
      isIdle = false;
      movementSpeed = Settings.runningSpeed;

      //Capture player direction for save game
      if(xInput < 0)
      {
        playerDirection = Direction.left;
      }
      else if(xInput > 0)
      {
        playerDirection = Direction.right;
      }
      else if(yInput < 0)
      {
        playerDirection = Direction.down;
      }
      else{
        playerDirection = Direction.up;
      }
    }
    else if (xInput == 0 && yInput == 0)
    {
      isRunning = false;
      isWalking = false;
      isIdle = true;
    }
  }

  private void PlayerWalkInput()
  {
    if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
      isRunning = false;
      isWalking = true;
      isIdle = false;
      movementSpeed = Settings.walkingSpeed;
    }
    else
    {
      isRunning = true;
      isWalking = false;
      isIdle = false;
      movementSpeed = Settings.runningSpeed;
    }
  }

  public Vector3 GetPlayerViewportPosition()
  {
    //Vector3 viewport position for player ((0,0) viewport bottom left, (1,1) viewport top right)
    return mainCamera.WorldToViewportPoint(transform.position);
  }

}
