using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPPgunInout : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private float lastMouseTrigger;

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            float ismousetrigger = UnityEngine.InputSystem.Mouse.current.leftButton.ReadValue();
            if (ismousetrigger > 0)
            {
                Debug.Log("鼠标单击" + ismousetrigger + "  " + UnityEngine.InputSystem.Mouse.current.leftButton.pressPoint);
            }
            lastMouseTrigger = ismousetrigger;
        }

        if(UnityEngine.InputSystem.Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("输入K Key");
        }
    }
}
