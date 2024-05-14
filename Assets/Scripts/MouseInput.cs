using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MouseInput : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.TriggerEvent(GameEvents.MOUSE_CLICK_DOWN, null);
            }
        }
    }
}