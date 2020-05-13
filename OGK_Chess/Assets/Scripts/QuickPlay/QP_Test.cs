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

            Debug.Log("Here");
            LayerMask mask = LayerMask.GetMask("Square");
            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if(hit.collider != null){
                Debug.Log("Hit something");
                if(hit.collider.GetComponent<Square>() != null){
                    Debug.Log("Hit square");
                    Square s = hit.collider.GetComponent<Square>();

                    // for now, just return that we've hit a square, and maybe print out it's coordinates.
                    _hitText.text = "Hit a square";
                }
            }
        }
    }
}
