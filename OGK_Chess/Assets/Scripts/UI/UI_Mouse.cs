/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class UI_Mouse : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(cursorPos.x, cursorPos.y);
    }
}
