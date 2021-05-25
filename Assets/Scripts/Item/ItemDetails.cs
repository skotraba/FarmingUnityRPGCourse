﻿
using UnityEngine;

[System.Serializable]

public class ItemDetails
{
    public int itemCode;
    public ItemType itemType;
    public string description;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius;
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickedUp;
    public bool canBeDropped;
    
    //MISC not implemented here
    public bool canBeEaten;
    public bool canBeCarried;


}
