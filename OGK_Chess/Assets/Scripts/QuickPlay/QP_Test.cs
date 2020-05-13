/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class QP_Test : MonoBehaviour
{
    public Text                 _text;
    public Text                 _hitText;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            _text.text = "Clicked!";

            LayerMask mask = LayerMask.GetMask("Square");
            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<Square>() != null){
                    Square s = hit.collider.GetComponent<Square>();

                    // for now, just return that we've hit a square, and maybe print out it's coordinates.
                    _hitText.text = "Hit a square";

                    Debug.Log(s._d._pos._x);
                    Debug.Log(s._d._PCE);
                }
            }
        }
    }
}
