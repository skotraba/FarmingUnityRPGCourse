using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{

    [SerializeField] private Sprite blank16Sprite = null;
    [SerializeField] private UIInventorySlot[] inventorySlot = null;
    public GameObject inventoryBarDraggedItem;

    [HideInInspector] public GameObject inventoryTextBoxGameobject;

    // Rect transform 
    private RectTransform rectTransform;

    private bool isInventoryBarPositionBottom = true;

    //Getter and setter for inventory bar position
    public bool IsInventoryBarPositionBottom { get => isInventoryBarPositionBottom; set => isInventoryBarPositionBottom = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // On enable method for subscribing
    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    //Because we have a subscription , we also need to unsubscribe in onDisable
    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    // Clear slots so we can just rebuild them when they change
    private void ClearInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].inventorySlotImage.sprite = blank16Sprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
                SetHighlightedInventorySlots(i);
            }
        }
    }

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {

        Debug.Log("Inventory Event");

        if(inventoryLocation == InventoryLocation.player)
        {
            ClearInventorySlots();

            if(inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                // loop through inventory slots and update with corresponding inventory list item
                for (int i = 0; i < inventorySlot.Length; i++ )
                {
                    if(i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;

                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if (itemDetails != null)
                        {
                           
                            // add images and details to inventory item slot
                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                            SetHighlightedInventorySlots(i);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    // Set the selected highlight if set on all invenotory item positions
    public void SetHighlightedInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            // loop through inventory slots and clear highlight sprites
            for (int i = 0; i < inventorySlot.Length; i++)
            {
                SetHighlightedInventorySlots(i);
            }
        }
    }

    // Overloaded method ^^^
    public void SetHighlightedInventorySlots(int itemPosition)
    {
        if (inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null)
        {
            if(inventorySlot[itemPosition].isSelected)
            {
                inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);

                // Update inventory to show item as selected
                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.itemCode);
            }
        }
    }

    private void Update()
    {
        //Switch inventory bar position
        SwitchInventoryBarPosition();
    }

    // Clear all highlights from the inventory Bar
    public void ClearHighlightOnInventorySlots()
    {
        if (inventorySlot.Length > 0)
        {
            //loop through inventory slots and clear highlight sprites
            for (int i = 0; i < inventorySlot.Length; i++)
            {
               if (inventorySlot[i].isSelected)
               {
                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);

                    //Updat inventory to show item as not selected
                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
               }
            }
        }
    }

    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();

        if(playerViewportPosition.y > 0.3f && IsInventoryBarPositionBottom == false)
        {
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);

            isInventoryBarPositionBottom = true;
        }
        else if(playerViewportPosition.y <= 0.3f && isInventoryBarPositionBottom == true)
        {
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);

            isInventoryBarPositionBottom = false;
        }
    }
}
