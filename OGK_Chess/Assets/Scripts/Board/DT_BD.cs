/*************************************************************************************
The board we play on.
*************************************************************************************/
using UnityEngine;

public struct DT_BD
{
    public DT_BD(int size = 8){
        _Squares = new DT_SQ[8,8];
        for(int y=0; y<8; y++){
            for(int x=0; x<8; x++){
                _Squares[x,y]._pos.x = x;
                _Squares[x,y]._pos.y = y;
                if((x+y)%2 == 1){       // if even then black, if odd then white
                    _Squares[x,y]._COL = SQUARE_COLOUR.WHITE;
                }else{
                    _Squares[x,y]._COL = SQUARE_COLOUR.BLACK;
                }
            }
        }
    }

    public DT_SQ[,]         _Squares;
}


public enum SQUARE_COLOUR{WHITE, BLACK};
public enum PIECE{EMPTY, WHITE_KING, BLACK_KING, WHITE_QUEEN, BLACK_QUEEN, WHITE_ROOK, BLACK_ROOK, WHITE_BISHOP, BLACK_BISHOP, WHITE_KNIGHT, BLACK_KNIGHT, WHITE_PAWN, BLACK_PAWN};
public struct DT_SQ
{
    public SQUARE_COLOUR            _COL;
    public Vector2Int               _pos;

    public PIECE                    _PCE;
}

