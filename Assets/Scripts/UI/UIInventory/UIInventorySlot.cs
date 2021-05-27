using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Camera mainCamera;
    private Transform parentItem;
    private GameObject draggedItem;

    private Canvas parentCanvas;

    [SerializeField]  private UIInventoryBar inventoryBar = null;
    [SerializeField] private GameObject itemPrefab = null;

    [SerializeField] private GameObject inventoryTextBoxPrefab = null;

    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;

    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuantity;

    [SerializeField] private int slotNumber = 0;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        parentItem = GameObject.FindGameObjectWithTag(Tags.itemsParentTransform).transform;
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            //Create item from prefab at mouse position
            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemCode = itemDetails.itemCode;

            // Remove item from players inventory
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (itemDetails != null)
        {
            //Disable keyboard
            Player.Instance.DisablePlayerInputAndResetMovement();

            // Instatiate gameobject as dragged item
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);

            // Get image for dragged item
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // move game object as dragged item
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Destroy game object as dragged item
        if (draggedItem != null)
        {
            Destroy(draggedItem);

            //If drag ends over inventory bar, get item drag is over and swap them
            if(eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                // get slot number where the drag ended
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;

                // Swap Inventory items in inventory list
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);

                //Destroy inventory text box
                DestroyInventoryTextBox();
            }
            else
            {
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }


            // Enable Player input
            Player.Instance.EnablePlayerInput();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Populate text box with item details
        if (itemQuantity != 0)
        {
            // Instantiate inventory text box
            inventoryBar.inventoryTextBoxGameobject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            inventoryBar.inventoryTextBoxGameobject.transform.SetParent(parentCanvas.transform, false);

            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameobject.GetComponent<UIInventoryTextBox>();

            // Set item type description
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);

            // Populate text box
            inventoryTextBox.SetTextBoxText(itemDetails.description, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");

            // Set text box position according to inventory bar position
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);

            }
            else
            {
                inventoryBar.inventoryTextBoxGameobject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameobject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextBox();
    }

    public void DestroyInventoryTextBox()
    {
        if (inventoryBar.inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryBar.inventoryTextBoxGameobject);
        }
    }


}
