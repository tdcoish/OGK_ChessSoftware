/*************************************************************************************
Interesting things to keep in mind when using Unity.
1) Pixels to units. When we import an art asset, by default Unity turns every 100 pixels 
into a single unit. I've changed this back to 1->1.
2) Camera "size" in 2D means pixels shown to above and below the cam. So a size of 512, shows a 
height of 1024.
3) The above gets complicated because width is somewhat free. So 1:1 means 1024x1024, but 16:9 means
1024 height, and 1820 width.
4) Note, the higher the pixels per unit, the smaller something will appear.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Square               PF_LightSquare;
    public Square               PF_DarkSquare;

    public Square[,]            _squares;

    public DT_BD                _data;

    public Piece                PF_WhiteKing;
    public Piece                PF_WhiteQueen;
    public Piece                PF_WhiteRook;
    public Piece                PF_WhiteBishop;
    public Piece                PF_WhiteKnight;
    public Piece                PF_WhitePawn;


    public Piece                PF_BlackKing;
    public Piece                PF_BlackQueen;
    public Piece                PF_BlackRook;
    public Piece                PF_BlackBishop;
    public Piece                PF_BlackKnight;
    public Piece                PF_BlackPawn;

    public Text                 _text;
    public Text                 _hitText;
    public Text                 _pieceText;

    public Text                 _textMoveLegal;
    public Text                 _textMoveList;

    public uint                 _selectedPiece;
    public bool                 _pieceSelected = false;
    public Vector2Int           _ixSelSq;

    // We shouldn't actually be rendering any pieces yet, we should be just storing the info in the squares.
    // Also, we need a reference to all these squares.

    void Start()
    {
        Init();
    }

    void Update()
    {
        HandleMovingPieces();

        RenderBoard();
    }

    public string ConvertMoveListToText(List<Vector2Int> moves)
    {
        if(moves == null){
            return "No moves selected, list null";
        }
        if(moves.Count == 0){
            return "Zero move count";
        }
        string sMoves = "";
        for(int i=0; i<moves.Count; i++)
        {
            // Plus one because internally board is 0,0 -> 7,7, but humans read as 1,1 -> 8,8
            sMoves += moves[i].x+1 + "," + (moves[i].y+1) + "\n";
        }
        return sMoves;
    }

    private void HandleMovingPieces()
    {
        if(_pieceSelected)
        {
            if(Input.GetMouseButtonDown(0)){
                HandleSelectingDeselecting(true);
            }
            _textMoveList.text = ConvertMoveListToText(ShowMoveListForSelectedPiece(_ixSelSq));
        }else {
            if(Input.GetMouseButtonDown(0)){
                HandleSelectingDeselecting(false);
            }
            _textMoveList.text = ConvertMoveListToText(ShowMoveListForSelectedPiece(_ixSelSq));
        }

    }

        // So they know which piece it is, and where it is.
    private List<Vector2Int> ShowMoveListForSelectedPiece(Vector2Int ix)
    {
        if(_squares[ix.x, ix.y]._d._uInfo == PieceInfo._noType){
            Debug.Log("No piece selected");
            return null;
        }

        List<Vector2Int> sMoves = new List<Vector2Int>();
        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._knight) == PieceInfo._knight){
            Vector2Int moveDest;
            moveDest = new Vector2Int(ix.x-2, ix.y+1);
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }

            moveDest = new Vector2Int((ix.x-2), (ix.y-1));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x+2), (ix.y+1));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x+2), (ix.y-1));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x -1), (ix.y - 2));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x -1), (ix.y + 2));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x +1), (ix.y - 2));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }
            moveDest = new Vector2Int((ix.x +1), (ix.y + 2));
            if(TestMoveLegit(moveDest, ix, true)){
                sMoves.Add(moveDest);
            }

            return sMoves;
        }

        // here we move all the way along a diagonal.
        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._bishop) == PieceInfo._bishop)
        {
            int xTemp = ix.x;
            int yTemp = ix.y;
            while(xTemp < 8 && yTemp < 8)
            {
                xTemp++; yTemp++;
                sMoves.Add(new Vector2Int(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp > 1 && yTemp > 1)
            {
                xTemp--; yTemp--; 
                sMoves.Add(new Vector2Int(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp > 1 && yTemp < 8)
            {
                xTemp--; yTemp++; 
                sMoves.Add(new Vector2Int(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp < 8 && yTemp > 1)
            {
                xTemp++; yTemp--; 
                sMoves.Add(new Vector2Int(xTemp, yTemp));
            }
            return sMoves;
        }

        // can move all the way in x or all the way in y
        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._rook) == PieceInfo._rook)
        {
            for(int x = 1; x<9; x++)
            {
                sMoves.Add(new Vector2Int(x, ix.y));
            }
            for(int y=1; y<9; y++){
                sMoves.Add(new Vector2Int(ix.x, y));
            }
            return sMoves;
        }

        // queen, maybe just return rook and bishop moves?

        // also, if we can capture, we can move diagonally.
        // pawn, have to factor in en passant, as well as two squares on first move.
        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._pawn) == PieceInfo._pawn)
        {
            sMoves.Add(new Vector2Int(ix.x, (ix.y+1)));
            // can't know en passant yet
            if(ix.y == 1){
                sMoves.Add(new Vector2Int(ix.x, (ix.y+2)));
            }
            return sMoves;
        }

        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._queen) == PieceInfo._queen)
        {
            Debug.Log("Queen not implemented yet");
            return null;
        }

        if((_squares[ix.x, ix.y]._d._uInfo & PieceInfo._king) == PieceInfo._king){
            int x = ix.x-1;
            sMoves.Add(new Vector2Int(x, (ix.y-1)));
            sMoves.Add(new Vector2Int(x, (ix.y+1)));
            sMoves.Add(new Vector2Int(x, (ix.y)));
            x += 2;
            sMoves.Add(new Vector2Int(x, (ix.y-1)));
            sMoves.Add(new Vector2Int(x, (ix.y+1)));
            sMoves.Add(new Vector2Int(x, (ix.y)));
            x = ix.x;
            sMoves.Add(new Vector2Int(x, (ix.y+1)));
            sMoves.Add(new Vector2Int(x, (ix.y-1)));
            return sMoves;
        }

        Debug.Log("error finding piece moves");
        return null;
    }

    // For now, only test if on board
    public bool TestMoveLegit(Vector2Int dest, Vector2Int cur, bool checkForOwnPiece = false)
    {
        bool moveIsLegit = true;
        if(dest.x < 0 || dest.x > 7 || dest.y < 0 || dest.y > 7)
        {
            return false;
        }

        uint pieceInfo = _squares[cur.x, cur.y]._d._uInfo;

        // now check if own piece is already on square
        bool ownPieceBlocking = false;
        // hack, won't let you capture enemy pieces.
        uint col = 0;
        if((pieceInfo & PieceInfo._white) == PieceInfo._white){
            col = PieceInfo._white;
        }
        else if((pieceInfo & PieceInfo._black) == PieceInfo._black){
            col = PieceInfo._black;
        }else{
            Debug.Log("No piece colour for this... explain");
        }

        if((_squares[dest.x, dest.y]._d._uInfo & col) == col){
            ownPieceBlocking = true;
        }
        if(checkForOwnPiece){
            moveIsLegit ^= ownPieceBlocking;
        }

        return moveIsLegit;
    }

    private void HandleSelectingDeselecting(bool curSelected)
    {
        if(curSelected)
        {
            Debug.Log("here");

            LayerMask mask = LayerMask.GetMask("Square");
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<Square>() != null){
                    Square s = hit.collider.GetComponent<Square>();

                    // If we have a piece selected, move it to the new square
                    if(_pieceSelected){
                        s._d._uInfo = _selectedPiece;
                        // now make the square of _ixSelSq not have that piece anymore.
                    }

                    Debug.Log(s._d._pos.x);
                    Debug.Log(s._d._pos.y);
                    Debug.Log(s._d._uInfo);
                    if(s._d._uInfo != PieceInfo._noType){
                        _pieceSelected = false;
                        _selectedPiece = PieceInfo._noType;
                        _squares[_ixSelSq.x, _ixSelSq.y]._d._uInfo = PieceInfo._noType;
                    }
                    _pieceText.text = "Piece Moved";
                }
            }
        }else{
            _text.text = "Clicked!";

            LayerMask mask = LayerMask.GetMask("Square");
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if(hit.collider != null){
                if(hit.collider.GetComponent<Square>() != null){
                    Square s = hit.collider.GetComponent<Square>();

                    // for now, just return that we've hit a square, and maybe print out it's coordinates.
                    _hitText.text = "Hit a square";

                    Debug.Log(s._d._pos.x);
                    Debug.Log(s._d._pos.y);
                    Debug.Log(s._d._uInfo);
                    if(s._d._uInfo != PieceInfo._noType){
                        _selectedPiece = s._d._uInfo;
                        _pieceSelected = true;
                        _ixSelSq.x = s._d._pos.x;
                        _ixSelSq.y = s._d._pos.y;
                    }
                    _pieceText.text = "Piece Selected : " + _selectedPiece;
                }
            }
        }
    }

    private void Init()
    {
        _data = new DT_BD(8);
        _squares = new Square[8,8];

        for(int y=0; y<8; y++)
        {
            for(int x=0; x<8; x++)
            {
                Vector2 pos = transform.position;
                // manually shifting the boxes around by pixel. Pivot is center, hence the 32.
                pos.x -= 256 - 32; pos.x += 64*x;
                pos.y -= 256 - 32; pos.y += 64*y;


                if(_data._Squares[x,y]._COL == SQUARE_COLOUR.WHITE)
                {
                    // spawn a light square here.
                    _squares[x,y] = Instantiate(PF_LightSquare, pos, transform.rotation);
                }
                else
                {
                    // spawn a dark square here.
                    _squares[x,y] = Instantiate(PF_DarkSquare, pos, transform.rotation);
                }

                _squares[x,y]._d._pos.x = x; _squares[x,y]._d._pos.y = y;
                _squares[x,y]._d._uInfo = 0;        // no piece type, no piece colour

                if(y == 0){
                    _squares[x,y]._d._uInfo |= PieceInfo._white;
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._uInfo |= PieceInfo._rook;
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._uInfo |= PieceInfo._knight;
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._uInfo |= PieceInfo._bishop;
                    }else if(x == 3){
                        _squares[x,y]._d._uInfo |= PieceInfo._queen;
                    }else if(x == 4){
                        _squares[x,y]._d._uInfo |= PieceInfo._king;
                    }
                }
                if(y == 1){
                    _squares[x,y]._d._uInfo |= PieceInfo._white;
                    _squares[x,y]._d._uInfo |= PieceInfo._pawn;
                }
                if(y == 6){
                    _squares[x,y]._d._uInfo |= PieceInfo._black;
                    _squares[x,y]._d._uInfo |= PieceInfo._pawn;
                }
                if(y == 7){
                    _squares[x,y]._d._uInfo |= PieceInfo._black;
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._uInfo |= PieceInfo._rook;
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._uInfo |= PieceInfo._knight;
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._uInfo |= PieceInfo._bishop;
                    }else if(x == 3){
                        _squares[x,y]._d._uInfo |= PieceInfo._queen;
                    }else if(x == 4){
                        _squares[x,y]._d._uInfo |= PieceInfo._king;
                    }
                }
            }
        }

        RenderBoard();
    }

    // Basically, delete all the pieces, then recreate them.
    void RenderBoard()
    {
        // destroy existing pieces.
        Piece[] pieces = FindObjectsOfType<Piece>();
        foreach(Piece p in pieces)
        {
            Destroy(p.gameObject);
        }

        // re-place new pieces.
        for(int y=0; y<8; y++)
        {
            for(int x=0; x<8; x++)
            {
                if((_squares[x,y]._d._uInfo & PieceInfo._noType) == PieceInfo._noType){
                    continue;
                }
                
                Vector2 pos = transform.position;
                // manually shifting the boxes around by pixel. Pivot is center, hence the 32.
                pos.x -= 256 - 32; pos.x += 64*x;
                pos.y -= 256 - 32; pos.y += 64*y;

                if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._pawn) == PieceInfo._pawn)){
                    Instantiate(PF_WhitePawn, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._bishop) == PieceInfo._bishop)){
                    Instantiate(PF_WhiteBishop, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._knight) == PieceInfo._knight)){
                    Instantiate(PF_WhiteKnight, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._rook) == PieceInfo._rook)){
                    Instantiate(PF_WhiteRook, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._queen) == PieceInfo._queen)){
                    Instantiate(PF_WhiteQueen, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._white) == PieceInfo._white) && ((_squares[x,y]._d._uInfo & PieceInfo._king) == PieceInfo._king)){
                    Instantiate(PF_WhiteKing, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._pawn) == PieceInfo._pawn)){
                    Instantiate(PF_BlackPawn, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._bishop) == PieceInfo._bishop)){
                    Instantiate(PF_BlackBishop, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._knight) == PieceInfo._knight)){
                    Instantiate(PF_BlackKnight, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._rook) == PieceInfo._rook)){
                    Instantiate(PF_BlackRook, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._queen) == PieceInfo._queen)){
                    Instantiate(PF_BlackQueen, pos, transform.rotation);
                } else if(((_squares[x,y]._d._uInfo & PieceInfo._black) == PieceInfo._black) && ((_squares[x,y]._d._uInfo & PieceInfo._king) == PieceInfo._king)){
                    Instantiate(PF_BlackKing, pos, transform.rotation);
                }
            }
        }
    }
}
