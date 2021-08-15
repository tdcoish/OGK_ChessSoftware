/*************************************************************************************
For now, maybe just text representation such as K for king, or N for knight. Possibly with a W or B prefixing this.


Ideally I'd have some logic running in the background, with the graphical layer purely as a final touch.



*************************************************************************************/
using UnityEngine;

public class BoardTest : MonoBehaviour
{
    public BoardSquareCenter                bottomLeft;
    public BoardSquareCenter                topRight;

    public GFX_Piece                        PF_KingGFX;
    // do need some logic for placing the pieces.
    public Vector2 calcPosOnScreen(Vector2Int pieceLoc)
    {
        // First, delete all extant graphics items.
        GFX_Piece[] pieces = FindObjectsOfType<GFX_Piece>();
        foreach(GFX_Piece p in pieces){
            Destroy(p.gameObject);
        }
        
        Vector2 pos = new Vector2();
        pos = bottomLeft.transform.position;

        // if it's x is 1, same as bot left. If x is 8, same as top right. If 4, somewhere in middle.
        // jump
        float gapSize = (topRight.transform.position.x - bottomLeft.transform.position.x) / 7;
        Debug.Log("Size of gap:" + gapSize);

        pos.x = bottomLeft.transform.position.x + (gapSize * (pieceLoc.x-1));
        pos.y = bottomLeft.transform.position.y + (gapSize * (pieceLoc.y-1));

        Instantiate(PF_KingGFX, pos, transform.rotation);
        return pos;
    }
}
