
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehavior<InventoryManager>
{
   //Create a dictionary to hold inventory items, makes items more accesible
   private Dictionary<int, ItemDetails> itemDetailsDictionary;

   [SerializeField] private SO_ItemList itemList = null;

  protected override void Awake()
  {
    base.Awake();

    //Create item details dictionary
    CreateItemDetailsDictionary();
  }

//    private void Start()
//    {
//        CreateItemDetailsDictionary();
//    }

   private void CreateItemDetailsDictionary()
   {
       itemDetailsDictionary = new Dictionary<int, ItemDetails>();

       foreach (ItemDetails itemDetails in itemList.itemDetails)
       {
           itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
       }
   }

   //Get item details for any item passed as parameter
   public ItemDetails GetItemDetails(int itemCode)
   {
       ItemDetails itemDetails;

       if(itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
       {
           return itemDetails;
       }
       else
       {
           return null;
       }
   }
}
