
using UnityEngine;

public class TriggerObscurringItemFader : MonoBehaviour
{
    //get game object we have collided with, and then get all the obscuring item fader components on it and it's children, and then trigger the fade out
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Getcomponentsinchildren does just that
        ObscurringItemFader[] obscurringItemFader = collision.gameObject.GetComponentsInChildren<ObscurringItemFader>();

        if(obscurringItemFader.Length > 0)
        {
            for(int i = 0; i < obscurringItemFader.Length; i ++)
            {
                obscurringItemFader[i].FadeOut();
            }
        }
    }

    //When player moves away from object
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Getcomponentsinchildren does just that
        ObscurringItemFader[] obscurringItemFader = collision.gameObject.GetComponentsInChildren<ObscurringItemFader>();

         if(obscurringItemFader.Length > 0)
        {
            for(int i = 0; i < obscurringItemFader.Length; i ++)
            {
                obscurringItemFader[i].FadeIn();
            }
        }
    }
   
}
