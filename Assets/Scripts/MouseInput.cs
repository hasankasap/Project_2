using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MouseInput : MonoBehaviour
    {
        bool firstClick = true;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (firstClick)
                {
                    EventManager.TriggerEvent(GameEvents.FIRST_CLICK, null);
                    firstClick = false;
                    //return;
                    // block this with return tutorial click or something this is our initial click
                }
                EventManager.TriggerEvent(GameEvents.MOUSE_CLICK_DOWN, null);
            }
        }
    }
}