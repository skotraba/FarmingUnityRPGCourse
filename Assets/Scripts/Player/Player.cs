using UnityEngine;
using System.Collections.Generic;

public class Player : SingletonMonoBehavior<Player>
{
  // Animation overrides
  private AnimationOverrides animationOverrides;


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

  private List<CharacterAttributes> characterAttributesCustomisatioinList;
  public float movementSpeed;

  [Tooltip("Should be populated in the prefab with the equipped item sprite renderer")]
  [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

  // Player attributes that can be swapped
  private CharacterAttributes armsCharacterAttribute;
  private CharacterAttributes toolCharacterAttribute;

  private bool _playerinputIsDisabled = false;

  public bool PlayerInputIsDisabled { get => _playerinputIsDisabled; set => _playerinputIsDisabled = value; }

  protected override void Awake()
  {
    base.Awake();

    rigidBody2D = GetComponent<Rigidbody2D>();

    animationOverrides = GetComponentInChildren<AnimationOverrides>();

    // Initalise swappable character attributes
    armsCharacterAttribute = new CharacterAttributes(CharacterPartAnimator.Arms, PartVariantColor.none, PartVariantType. none);

    // Initalise character attribute list
    characterAttributesCustomisatioinList = new List<CharacterAttributes>();

    // Get reference to camera
    mainCamera = Camera.main;
  }

  private void Update()
  {
    #region Player Input

    if(!PlayerInputIsDisabled)
    {
      ResetAnimationTriggers();

      PlayerMovementInput();

      PlayerWalkInput();

      PlayerTestInput();

      EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
        isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
        isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
        isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
        isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
        false, false, false, false);
      
    }


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

  public void ClearCarriedItem()
  {
    equippedItemSpriteRenderer.sprite = null;
    equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

    // Apply base character arms customisation
    armsCharacterAttribute.partVariantType = PartVariantType.none;
    characterAttributesCustomisatioinList.Clear();
    characterAttributesCustomisatioinList.Add(armsCharacterAttribute);
    animationOverrides.ApplyCharacterCustomisationParameters(characterAttributesCustomisatioinList);

    isCarrying = false;
  }

  public void ShowCarriedItem(int itemCode)
  {
    ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
    if (itemDetails != null)
    {
      equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
      equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

      //Apply 'carry' character arms customisation
      armsCharacterAttribute.partVariantType = PartVariantType.carry;
      characterAttributesCustomisatioinList.Clear();
      characterAttributesCustomisatioinList.Add(armsCharacterAttribute);
      animationOverrides.ApplyCharacterCustomisationParameters(characterAttributesCustomisatioinList);

      isCarrying = true;
    }
  }


  public Vector3 GetPlayerViewportPosition()
  {
    //Vector3 viewport position for player ((0,0) viewport bottom left, (1,1) viewport top right)
    return mainCamera.WorldToViewportPoint(transform.position);
  }

  // Check for key presses
  private void PlayerTestInput()
  {
    // Trigger advance time
    if (Input.GetKey(KeyCode.T))
    {
      TimeManager.Instance.TestAdvanceGameMinute();
    }

    // Trigger advance dat
    if (Input.GetKey(KeyCode.G))
    {
      TimeManager.Instance.TestAdvanceGameDay();
    }

    // Test scene unload / load
    if (Input.GetKeyDown(KeyCode.L))
    {
      SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene1_Farm.ToString(), transform.position);
    }
  }

  private void ResetMovement()
  {
    xInput = 0f;
    yInput = 0f;
    isRunning = false;
    isWalking = false;
    isIdle = true;
  }

  public void DisablePlayerInputAndResetMovement()
  {
    DisablePlayerInput();
    ResetMovement();

    //Send event to any listeners for player movement input
     EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
        isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
        isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
        isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
        isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
        false, false, false, false);
  }

  public void EnablePlayerInput()
  {
    PlayerInputIsDisabled = false;
  }

  public void DisablePlayerInput()
  {
    PlayerInputIsDisabled = true;
  }

}
