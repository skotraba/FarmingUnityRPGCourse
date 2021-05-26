using System;
using System.Collections.Generic;
using UnityEngine;

//Delegate is reference type that holds reference to methods
public delegate void MovementDelegate(
    float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight
    );

public static class EventHandler
{

    //Inventory Update Event
    
    //Action is basically a system defined delegate
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;

    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if(InventoryUpdatedEvent != null)
        {
            
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
        }
    }



    //Movement Event Subscribers

    //Events are wrappers around delegates that allow us to subscribe and unsubscribe from delegates
    public static event MovementDelegate MovementEvent;




    //Movement Event Call publishers
    public static void CallMovementEvent(float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight)
    {
        // Check if there are subscribers, only want to execute event if there are subscribers
        // if(MovementEvent != null)
        // {
        //     MovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
        //     isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
        //     isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
        //     isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
        //     isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
        //     idleUp, idleDown, idleLeft, idleRight);
        // }


        // Can shorten above with null conditional operator
        MovementEvent?.Invoke(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            idleUp, idleDown, idleLeft, idleRight);
    }
}