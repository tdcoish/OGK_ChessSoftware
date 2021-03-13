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

    public PIECE                _selectedPiece;
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

    public string ConvertMoveListToText(List<SquareXY> moves)
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
            sMoves += moves[i].x + "," + moves[i].y + "\n";
        }
        return sMoves;
    }

    private void HandleMovingPieces()
    {
        if(_pieceSelected)
        {
            if(Input.GetMouseButtonDown(0)){
                HandleSelecingDeselecting(true);
            }
            _textMoveList.text = ConvertMoveListToText(ShowMoveListForSelectedPiece(_selectedPiece, _ixSelSq));
        }else {
            if(Input.GetMouseButtonDown(0)){
                HandleSelecingDeselecting(false);
            }
            _textMoveList.text = ConvertMoveListToText(ShowMoveListForSelectedPiece(_selectedPiece, _ixSelSq));
        }

    }

        // So they know which piece it is, and where it is.
    private List<SquareXY> ShowMoveListForSelectedPiece(PIECE piece, Vector2Int ix)
    {
        if(piece == PIECE.EMPTY){
            Debug.Log("No piece selected");
            return null;
        }

        // have to go from 0-7 to 1-8
        ix.x += 1;
        ix.y += 1;

        Debug.Log(ix);

        List<SquareXY> sMoves = new List<SquareXY>();
        if(piece == PIECE.WHITE_KNIGHT){
            SquareXY move;
            move = new SquareXY(ix.x-2, ix.y+1);
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }

            move = new SquareXY((ix.x-2), (ix.y-1));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x+2), (ix.y+1));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x+2), (ix.y-1));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x -1), (ix.y - 2));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x -1), (ix.y + 2));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x +1), (ix.y - 2));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }
            move = new SquareXY((ix.x +1), (ix.y + 2));
            if(TestMoveLegit(move)){
                sMoves.Add(move);
            }

            return sMoves;
        }

        // here we move all the way along a diagonal.
        if(piece == PIECE.WHITE_BISHOP)
        {
            int xTemp = ix.x;
            int yTemp = ix.y;
            while(xTemp < 8 && yTemp < 8)
            {
                xTemp++; yTemp++;
                sMoves.Add(new SquareXY(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp > 1 && yTemp > 1)
            {
                xTemp--; yTemp--; 
                sMoves.Add(new SquareXY(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp > 1 && yTemp < 8)
            {
                xTemp--; yTemp++; 
                sMoves.Add(new SquareXY(xTemp, yTemp));
            }
            xTemp = ix.x;
            yTemp = ix.y;
            while(xTemp < 8 && yTemp > 1)
            {
                xTemp++; yTemp--; 
                sMoves.Add(new SquareXY(xTemp, yTemp));
            }
            return sMoves;
        }

        // can move all the way in x or all the way in y
        if(piece == PIECE.WHITE_ROOK)
        {
            for(int x = 1; x<9; x++)
            {
                sMoves.Add(new SquareXY(x, ix.y));
            }
            for(int y=1; y<9; y++){
                sMoves.Add(new SquareXY(ix.x, y));
            }
            return sMoves;
        }

        // queen, maybe just return rook and bishop moves?

        // also, if we can capture, we can move diagonally.
        // pawn, have to factor in en passant, as well as two squares on first move.
        if(piece == PIECE.WHITE_PAWN)
        {
            sMoves.Add(new SquareXY(ix.x, (ix.y+1)));
            // can't know en passant yet
            if(ix.y == 2){
                sMoves.Add(new SquareXY(ix.x, (ix.y+2)));
            }
            return sMoves;
        }

        if(piece == PIECE.WHITE_QUEEN)
        {
            Debug.Log("Queen not implemented yet");
            return null;
        }

        if(piece == PIECE.WHITE_KING){
            int x = ix.x-1;
            sMoves.Add(new SquareXY(x, (ix.y-1)));
            sMoves.Add(new SquareXY(x, (ix.y+1)));
            sMoves.Add(new SquareXY(x, (ix.y)));
            x += 2;
            sMoves.Add(new SquareXY(x, (ix.y-1)));
            sMoves.Add(new SquareXY(x, (ix.y+1)));
            sMoves.Add(new SquareXY(x, (ix.y)));
            x = ix.x;
            sMoves.Add(new SquareXY(x, (ix.y+1)));
            sMoves.Add(new SquareXY(x, (ix.y-1)));
            return sMoves;
        }

        Debug.Log("error finding piece moves");
        return null;
    }

    // For now, only test if on board
    public bool TestMoveLegit(SquareXY move)
    {
        if(move.x >= 0 && move.x < 8)
        {
            if(move.y >= 0 && move.y < 8)
            {
                return true;
            }
        }
        return false;
    }

    // // So they know which piece it is, and where it is.
    // private string ShowMoveListForSelectedPiece(PIECE piece, Vector2Int ix)
    // {
    //     if(piece == PIECE.EMPTY){
    //         return "No piece selected";
    //     }

    //     // have to go from 0-7 to 1-8
    //     ix.x += 1;
    //     ix.y += 1;

    //     Debug.Log(ix);

    //     string sMoves = "";
    //     if(piece == PIECE.WHITE_KNIGHT){

    //         sMoves += (ix.x-2) + "," + (ix.y+1);
    //         sMoves += "\n" + (ix.x-2) + "," + (ix.y-1);
    //         sMoves += "\n" + (ix.x+2) + "," + (ix.y+1);
    //         sMoves += "\n" + (ix.x+2) + "," + (ix.y-1);
    //         sMoves += "\n" + (ix.x - 1) + "," + (ix.y - 2);
    //         sMoves += "\n" + (ix.x - 1) + "," + (ix.y + 2);
    //         sMoves += "\n" + (ix.x + 1) + "," + (ix.y - 2);
    //         sMoves += "\n" + (ix.x + 1) + "," + (ix.y + 2);

    //         return sMoves;
    //     }

    //     // here we move all the way along a diagonal.
    //     if(piece == PIECE.WHITE_BISHOP)
    //     {
    //         int xTemp = ix.x;
    //         int yTemp = ix.y;
    //         while(xTemp < 8 && yTemp < 8)
    //         {
    //             xTemp++; yTemp++;
    //             sMoves += "\n" + xTemp + "," + yTemp;
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp > 1 && yTemp > 1)
    //         {
    //             xTemp--; yTemp--; 
    //             sMoves += "\n" + xTemp + "," + yTemp;
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp > 1 && yTemp < 8)
    //         {
    //             xTemp--; yTemp++; 
    //             sMoves += "\n" + xTemp + "," + yTemp;
    //         }
    //         xTemp = ix.x;
    //         yTemp = ix.y;
    //         while(xTemp < 8 && yTemp > 1)
    //         {
    //             xTemp++; yTemp--; 
    //             sMoves += "\n" + xTemp + "," + yTemp;
    //         }
    //         return sMoves;
    //     }

    //     // can move all the way in x or all the way in y
    //     if(piece == PIECE.WHITE_ROOK)
    //     {
    //         for(int x = 1; x<9; x++)
    //         {
    //             sMoves += x + "," + ix.y + "\n";
    //         }
    //         for(int y=1; y<9; y++){
    //             sMoves += ix.x + "," + y + "\n";
    //         }
    //         return sMoves;
    //     }

    //     // queen, maybe just return rook and bishop moves?

    //     // also, if we can capture, we can move diagonally.
    //     // pawn, have to factor in en passant, as well as two squares on first move.
    //     if(piece == PIECE.WHITE_PAWN)
    //     {
    //         sMoves += ix.x + "," + (ix.y+1) + "\n";
    //         // can't know en passant yet
    //         if(ix.y == 2){
    //             sMoves += ix.x + "," + (ix.y+2) + "\n";
    //         }
    //         return sMoves;
    //     }

    //     if(piece == PIECE.WHITE_QUEEN)
    //     {
    //         return "rook plus bishop";
    //     }

    //     if(piece == PIECE.WHITE_KING){
    //         int x = ix.x-1;
    //         sMoves += x + "," + (ix.y-1) + "\n";
    //         sMoves += x + "," + (ix.y+1) + "\n";
    //         sMoves += x + "," + (ix.y) + "\n";
    //         x += 2;
    //         sMoves += x + "," + (ix.y-1) + "\n";
    //         sMoves += x + "," + (ix.y+1) + "\n";
    //         sMoves += x + "," + (ix.y) + "\n";
    //         x = ix.x;
    //         sMoves += x + "," + (ix.y+1) + "\n";
    //         sMoves += x + "," + (ix.y-1) + "\n";
    //         return sMoves;
    //     }


    //     return "error finding piece moves";
    // }

    private void HandleSelecingDeselecting(bool curSelected)
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
                        s._d._PCE = _selectedPiece;
                        // now make the square of _ixSelSq not have that piece anymore.
                    }

                    Debug.Log(s._d._pos.x);
                    Debug.Log(s._d._pos.y);
                    Debug.Log(s._d._PCE);
                    if(s._d._PCE != PIECE.EMPTY){
                        _pieceSelected = false;
                        _selectedPiece = PIECE.EMPTY;
                        _squares[_ixSelSq.x, _ixSelSq.y]._d._PCE = PIECE.EMPTY;
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
                    Debug.Log(s._d._PCE);
                    if(s._d._PCE != PIECE.EMPTY){
                        _selectedPiece = s._d._PCE;
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
                _squares[x,y]._d._PCE = PIECE.EMPTY;

                if(y == 0){
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._PCE = PIECE.WHITE_ROOK;
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._PCE = PIECE.WHITE_KNIGHT;
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._PCE = PIECE.WHITE_BISHOP;
                    }else if(x == 3){
                        _squares[x,y]._d._PCE = PIECE.WHITE_QUEEN;
                    }else if(x == 4){
                        _squares[x,y]._d._PCE = PIECE.WHITE_KING;
                    }
                }
                if(y == 1){
                    _squares[x,y]._d._PCE = PIECE.WHITE_PAWN;
                }
                if(y == 6){
                    _squares[x,y]._d._PCE = PIECE.BLACK_PAWN;
                }
                if(y == 7){
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._PCE = PIECE.BLACK_ROOK;
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._PCE = PIECE.BLACK_KNIGHT;
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._PCE = PIECE.BLACK_BISHOP;
                    }else if(x == 3){
                        _squares[x,y]._d._PCE = PIECE.BLACK_QUEEN;
                    }else if(x == 4){
                        _squares[x,y]._d._PCE = PIECE.BLACK_KING;
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
                if(_squares[x,y]._d._PCE == PIECE.EMPTY){
                    continue;
                }
                
                Vector2 pos = transform.position;
                // manually shifting the boxes around by pixel. Pivot is center, hence the 32.
                pos.x -= 256 - 32; pos.x += 64*x;
                pos.y -= 256 - 32; pos.y += 64*y;

                if(_squares[x,y]._d._PCE == PIECE.WHITE_PAWN){
                    Instantiate(PF_WhitePawn, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.WHITE_BISHOP){
                    Instantiate(PF_WhiteBishop, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.WHITE_KNIGHT){
                    Instantiate(PF_WhiteKnight, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.WHITE_ROOK){
                    Instantiate(PF_WhiteRook, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.WHITE_QUEEN){
                    Instantiate(PF_WhiteQueen, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.WHITE_KING){
                    Instantiate(PF_WhiteKing, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_PAWN){
                    Instantiate(PF_BlackPawn, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_BISHOP){
                    Instantiate(PF_BlackBishop, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_KNIGHT){
                    Instantiate(PF_BlackKnight, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_ROOK){
                    Instantiate(PF_BlackRook, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_QUEEN){
                    Instantiate(PF_BlackQueen, pos, transform.rotation);
                } else if(_squares[x,y]._d._PCE == PIECE.BLACK_KING){
                    Instantiate(PF_BlackKing, pos, transform.rotation);
                }
            }
        }
    }
}
