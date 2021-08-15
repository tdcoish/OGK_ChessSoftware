/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // List<Piece>                  rPieces;
    // Board                       rBoard;

    // public Text                 _text;
    // public Text                 _hitText;
    // public Text                 _pieceText;

    // public Text                 _textMoveLegal;
    // public Text                 _textMoveList;

    // public int                  _selectedPiece;
    // public bool                 _pieceSelected = false;
    // public Vector2Int           _ixSelSq;

    // public bool                 _whiteTurn;         

    // void Start()
    // {
    //     rPieces = new List<Piece>();
    //     rBoard = FindObjectOfType<Board>();
    //     rBoard.Init();

    //     // Now spawn pieces depending on the board values.
        

    //     rPieces = new List<Piece>(FindObjectsOfType<Piece>());
    // }

    // void Update()
    // {
    //     HandleMovingPieces();
    //     rBoard.RenderBoard();
    // }

    // // Should have our logic for moving pieces and stuffs.
    
    // private void HandleMovingPieces()
    // {
    //     if(Input.GetMouseButtonDown(0)){
    //         HandleSelectingDeselecting(_pieceSelected);
    //     }
    //     _textMoveList.text = ConvertMoveListToText(ShowMoveListForSelectedPiece(_ixSelSq));
    // }

    // public string ConvertMoveListToText(List<Vector2Int> moves)
    // {
    //     if(moves == null){
    //         return "No moves selected, list null";
    //     }
    //     if(moves.Count == 0){
    //         return "Zero move count";
    //     }
    //     string sMoves = "";
    //     for(int i=0; i<moves.Count; i++)
    //     {
    //         // Plus one because internally board is 0,0 -> 7,7, but humans read as 1,1 -> 8,8
    //         sMoves += moves[i].x+1 + "," + (moves[i].y+1) + "\n";
    //     }
    //     return sMoves;
    // }

    //     // So they know which piece it is, and where it is.
    // private List<Vector2Int> ShowMoveListForSelectedPiece(Vector2Int ix)
    // {
    //     if(rBoard._squares[ix.x, ix.y]._d._uInfo == PieceInfo._noType){
    //         Debug.Log("No piece selected");
    //         return null;
    //     }

    //     List<Vector2Int> sMoves = new List<Vector2Int>();
    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._knight) == PieceInfo._knight){
    //         Vector2Int moveDest;
    //         moveDest = new Vector2Int(ix.x-2, ix.y+1);
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }

    //         moveDest = new Vector2Int((ix.x-2), (ix.y-1));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x+2), (ix.y+1));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x+2), (ix.y-1));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x -1), (ix.y - 2));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x -1), (ix.y + 2));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x +1), (ix.y - 2));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }
    //         moveDest = new Vector2Int((ix.x +1), (ix.y + 2));
    //         if(TestMoveLegit(moveDest, ix, true)){
    //             sMoves.Add(moveDest);
    //         }

    //         return sMoves;
    //     }

    //     // here we move all the way along a diagonal.
    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._bishop) == PieceInfo._bishop)
    //     {
    //         int xTemp = ix.x;
    //         int yTemp = ix.y;
    //         while(xTemp < 8 && yTemp < 8)
    //         {
    //             xTemp++; yTemp++;
    //             sMoves.Add(new Vector2Int(xTemp, yTemp));
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp > 1 && yTemp > 1)
    //         {
    //             xTemp--; yTemp--; 
    //             sMoves.Add(new Vector2Int(xTemp, yTemp));
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp > 1 && yTemp < 8)
    //         {
    //             xTemp--; yTemp++; 
    //             sMoves.Add(new Vector2Int(xTemp, yTemp));
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp < 8 && yTemp > 1)
    //         {
    //             xTemp++; yTemp--; 
    //             sMoves.Add(new Vector2Int(xTemp, yTemp));
    //         }
    //         return sMoves;
    //     }

    //     // can move all the way in x or all the way in y
    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._rook) == PieceInfo._rook)
    //     {
    //         for(int x = 1; x<9; x++)
    //         {
    //             sMoves.Add(new Vector2Int(x, ix.y));
    //         }
    //         for(int y=1; y<9; y++){
    //             sMoves.Add(new Vector2Int(ix.x, y));
    //         }
    //         return sMoves;
    //     }

    //     // queen, maybe just return rook and bishop moves?

    //     // also, if we can capture, we can move diagonally.
    //     // pawn, have to factor in en passant, as well as two squares on first move.
    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._pawn) == PieceInfo._pawn)
    //     {
    //         sMoves.Add(new Vector2Int(ix.x, (ix.y+1)));
    //         // can't know en passant yet
    //         if(ix.y == 1){
    //             sMoves.Add(new Vector2Int(ix.x, (ix.y+2)));
    //         }
    //         return sMoves;
    //     }

    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._queen) == PieceInfo._queen)
    //     {
    //         Debug.Log("Queen not implemented yet");
    //         return null;
    //     }

    //     if((rBoard._squares[ix.x, ix.y]._d._uInfo & PieceInfo._king) == PieceInfo._king){
    //         int x = ix.x-1;
    //         sMoves.Add(new Vector2Int(x, (ix.y-1)));
    //         sMoves.Add(new Vector2Int(x, (ix.y+1)));
    //         sMoves.Add(new Vector2Int(x, (ix.y)));
    //         x += 2;
    //         sMoves.Add(new Vector2Int(x, (ix.y-1)));
    //         sMoves.Add(new Vector2Int(x, (ix.y+1)));
    //         sMoves.Add(new Vector2Int(x, (ix.y)));
    //         x = ix.x;
    //         sMoves.Add(new Vector2Int(x, (ix.y+1)));
    //         sMoves.Add(new Vector2Int(x, (ix.y-1)));
    //         return sMoves;
    //     }

    //     Debug.Log("error finding piece moves");
    //     return null;
    // }

    // // For now, only test if on board
    // public bool TestMoveLegit(Vector2Int dest, Vector2Int cur, bool checkForOwnPiece = false)
    // {
    //     bool moveIsLegit = true;
    //     if(dest.x < 0 || dest.x > 7 || dest.y < 0 || dest.y > 7)
    //     {
    //         return false;
    //     }

    //     uint pieceInfo = rBoard._squares[cur.x, cur.y]._d._uInfo;

    //     // now check if own piece is already on square
    //     bool ownPieceBlocking = false;
    //     // hack, won't let you capture enemy pieces.
    //     uint col = 0;
    //     if((pieceInfo & PieceInfo._white) == PieceInfo._white){
    //         col = PieceInfo._white;
    //     }
    //     else if((pieceInfo & PieceInfo._black) == PieceInfo._black){
    //         col = PieceInfo._black;
    //     }else{
    //         Debug.Log("No piece colour for this... explain");
    //     }

    //     if((rBoard._squares[dest.x, dest.y]._d._uInfo & col) == col){
    //         ownPieceBlocking = true;
    //     }
    //     if(checkForOwnPiece){
    //         moveIsLegit ^= ownPieceBlocking;
    //     }

    //     return moveIsLegit;
    // }

    // // All we know is the user pressed LMB. We have to find out what to do with this now.
    // private void HandleSelectingDeselecting(bool pieceAlreadySelected)
    // {
    //     if(pieceAlreadySelected)
    //     {
    //         LayerMask mask = LayerMask.GetMask("Square");
    //         Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
    //         if(hit.collider != null){
    //             if(hit.collider.GetComponent<Square>() != null){
    //                 Square s = hit.collider.GetComponent<Square>();

    //                 // Alrighty. Here we have finally found they hit a square. New question is: Do we let them move?
    //                 // If we have a piece selected, move it to the new square
    //                 // Check if move legal.
    //                 if(TestMoveLegit(s._d._pos, _ixSelSq, true)){
    //                     s._d._uInfo = _selectedPiece;
    //                 }else{
    //                     Debug.Log("Can't make that move for some reason");
    //                 }

    //                 Debug.Log(s._d._pos.x);
    //                 Debug.Log(s._d._pos.y);
    //                 Debug.Log(s._d._uInfo);
    //                 if(s._d._uInfo != PieceInfo._noType){
    //                     _pieceSelected = false;
    //                     _selectedPiece = PieceInfo._noType;
    //                     rBoard._squares[_ixSelSq.x, _ixSelSq.y]._d._uInfo = PieceInfo._noType;
    //                 }
    //                 _pieceText.text = "Piece Moved";
    //             }
    //         }
    //     }else{
    //         _text.text = "Clicked!";

    //         LayerMask mask = LayerMask.GetMask("Square");
    //         Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
    //         if(hit.collider != null){
    //             if(hit.collider.GetComponent<Square>() != null){
    //                 Square s = hit.collider.GetComponent<Square>();

    //                 // for now, just return that we've hit a square, and maybe print out it's coordinates.
    //                 _hitText.text = "Hit a square";

    //                 Debug.Log(s._d._pos.x);
    //                 Debug.Log(s._d._pos.y);
    //                 Debug.Log(s._d._uInfo);
    //                 if(s._d._uInfo != PieceInfo._noType){
    //                     _selectedPiece = s._d._uInfo;
    //                     _pieceSelected = true;
    //                     _ixSelSq.x = s._d._pos.x;
    //                     _ixSelSq.y = s._d._pos.y;
    //                 }
    //                 _pieceText.text = "Piece Selected : " + ConvertPieceInfoToString(_selectedPiece);
    //             }
    //         }
    //     }
    // }

    // public string ConvertPieceInfoToString(uint info)
    // {
    //     string sInfo = "";
    //     if((info & PieceInfo._white) == PieceInfo._white){
    //         sInfo += "White ";
    //     }
    //     if((info & PieceInfo._black) == PieceInfo._black){
    //         sInfo += "Black ";
    //     }
    //     if((info & PieceInfo._pawn) == PieceInfo._pawn){
    //         sInfo += "pawn";
    //     }
    //     if((info & PieceInfo._knight) == PieceInfo._knight){
    //         sInfo += "knight";
    //     }        
    //     if((info & PieceInfo._bishop) == PieceInfo._bishop){
    //         sInfo += "bishop";
    //     }        
    //     if((info & PieceInfo._rook) == PieceInfo._rook){
    //         sInfo += "rook";
    //     }           
    //     if((info & PieceInfo._queen) == PieceInfo._queen){
    //         sInfo += "queen";
    //     }        
    //     if((info & PieceInfo._king) == PieceInfo._king){
    //         sInfo += "king";
    //     }

    //     return sInfo;
    // }

}