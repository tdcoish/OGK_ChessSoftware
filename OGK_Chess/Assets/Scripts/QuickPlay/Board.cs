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

    // We shouldn't actually be rendering any pieces yet, we should be just storing the info in the squares.
    // Also, we need a reference to all these squares.

    void Start()
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

                _squares[x,y]._d._pos._x = x; _squares[x,y]._d._pos._y = y;
                _squares[x,y]._d._PCE = PIECE.EMPTY;

                if(y == 0){
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._PCE = PIECE.WHITE_ROOK;
                        Instantiate(PF_WhiteRook, pos, transform.rotation);
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._PCE = PIECE.WHITE_KNIGHT;
                        Instantiate(PF_WhiteKnight, pos, transform.rotation);
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._PCE = PIECE.WHITE_BISHOP;
                        Instantiate(PF_WhiteBishop, pos, transform.rotation);
                    }else if(x == 3){
                        _squares[x,y]._d._PCE = PIECE.WHITE_QUEEN;
                        Instantiate(PF_WhiteQueen, pos, transform.rotation);
                    }else if(x == 4){
                        _squares[x,y]._d._PCE = PIECE.WHITE_KING;
                        Instantiate(PF_WhiteKing, pos, transform.rotation);
                    }
                }
                if(y == 1){
                    Instantiate(PF_WhitePawn, pos, transform.rotation);
                    _squares[x,y]._d._PCE = PIECE.WHITE_PAWN;
                }
                if(y == 6){
                    _squares[x,y]._d._PCE = PIECE.BLACK_PAWN;
                    Instantiate(PF_BlackPawn, pos, transform.rotation);
                }
                if(y == 7){
                    if(x == 0 || x == 7){
                        _squares[x,y]._d._PCE = PIECE.BLACK_ROOK;
                        Instantiate(PF_BlackRook, pos, transform.rotation);
                    }else if(x == 1 || x == 6){
                        _squares[x,y]._d._PCE = PIECE.BLACK_KNIGHT;
                        Instantiate(PF_BlackKnight, pos, transform.rotation);
                    }else if(x == 2 || x == 5){
                        _squares[x,y]._d._PCE = PIECE.BLACK_BISHOP;
                        Instantiate(PF_BlackBishop, pos, transform.rotation);
                    }else if(x == 3){
                        _squares[x,y]._d._PCE = PIECE.BLACK_QUEEN;
                        Instantiate(PF_BlackQueen, pos, transform.rotation);
                    }else if(x == 4){
                        _squares[x,y]._d._PCE = PIECE.BLACK_KING;
                        Instantiate(PF_BlackKing, pos, transform.rotation);
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
