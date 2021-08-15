/*************************************************************************************
Putting the logic board in here.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    // test that king moves are legal.
    // start with the king on a certain position, let's say 44.

    public BoardTest                rBoardGFX;

    public string[,]                board;


    public Text                     txt_LegalMoves;
    public Text                     txt_KingPos;

    Vector2Int                      kingPos;
    List<Vector2Int>                legalMoves = new List<Vector2Int>();

    public void Start()
    {
        board = new string[8,8];
        kingPos = new Vector2Int(5,1);

        legalMoves = FindLegalMoves(kingPos);
        rBoardGFX.calcPosOnScreen(kingPos);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // choose a move at random.
            int index = Random.Range(0, legalMoves.Count);

            Debug.Log("New move");
            kingPos = legalMoves[index];
            legalMoves = FindLegalMoves(kingPos);
            rBoardGFX.calcPosOnScreen(kingPos);
        }
    }



    // kings move up down left right diagonal. Should be 8 legal moves.
    public List<Vector2Int> FindLegalMoves(Vector2Int pos)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int move = new Vector2Int();
        move = pos;
        // move to the left.
        move.x -= 1;
        moves.Add(move);
        move = pos;
        // move to the right.
        move.x += 1;
        moves.Add(move);
        move = pos;
        move.y += 1;
        moves.Add(move);
        move = pos;
        move.y -= 1;
        moves.Add(move);

        move = pos;
        move.x -= 1;
        move.y -= 1;
        moves.Add(move);
        move = pos;
        move.x -= 1;
        move.y += 1;
        moves.Add(move);
        move = pos;
        move.x += 1;
        move.y -= 1;
        moves.Add(move);
        move = pos;
        move.x += 1;
        move.y += 1;
        moves.Add(move);

        txt_KingPos.text = "King Position: " + pos;

        txt_LegalMoves.text = "Legal King Moves\n";
        for(int i=0; i<moves.Count; i++)
        {
            if(!CheckMoveLegal(moves[i])){
                Debug.Log("Move:" + moves[i] + "illegal");
                moves.RemoveAt(i);
                i--;
            }else{
                txt_LegalMoves.text += moves[i] + "\n";
                // Debug.Log(moves[i]);
            }
        }

        return moves;
    }

    // For now, just if move is on board.
    private bool CheckMoveLegal(Vector2Int pos)
    {
        if(pos.x <= 0 || pos.x > 8){
            return false;
        }
        if(pos.y <= 0 || pos.y > 8){
            return false;
        }
        return true;
    }
}

