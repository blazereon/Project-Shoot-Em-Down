using Unity.VisualScripting;
using UnityEngine;

public class EventSystem
{
    private static EventSystem current = null;

    private EventSystem() { }

    public static EventSystem Current 
    {
        get 
        {
            if (current == null)
            {
                current = new EventSystem();
            }

            return current;
        }

        //Insert event related code below
    }

}
