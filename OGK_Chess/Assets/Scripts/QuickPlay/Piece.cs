/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public enum SIDE{WHITE, BLACK};
public enum PIECE_TYPE{PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING};

public class Piece : MonoBehaviour
{
    public SIDE                 _side;
    public PIECE_TYPE           _type;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
