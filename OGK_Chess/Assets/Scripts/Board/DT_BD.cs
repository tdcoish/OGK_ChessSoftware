/*************************************************************************************
The board we play on.
*************************************************************************************/
using UnityEngine;

public static class PieceInfo{
    public static uint           _white = 1<<1;
    public static uint           _black = 1<<2;
    public static uint           _knight = 1<<3;
    public static uint           _bishop = 1<<4;
    public static uint           _rook = 1<<5;
    public static uint           _queen = 1<<6;
    public static uint           _king = 1<<7;
    public static uint           _pawn = 1<<8;
    public static uint           _noType = 1<<9;
}

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

public struct DT_SQ
{
    public SQUARE_COLOUR            _COL;
    public Vector2Int               _pos;

    // Weird. This is the info for the piece on the square. Poor way of doing things.
    public uint                     _uInfo;
}

