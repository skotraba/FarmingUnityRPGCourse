//Delegate is reference type that holds reference to methods
public delegate void MovementDelegate(
    float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying, ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idleLeft, bool idleRight
    );

// += is used to subscribe to an event, -= is used to unsubscribe to an event.

public static class EventHandler
{
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
        if(MovementEvent != null)
        {
            MovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect, 
            isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
            isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
            isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
            isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
            idleUp, idleDown, idleLeft, idleRight);
        }
    }
}