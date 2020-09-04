/*************************************************************************************
For at least en-passants we need to know the previous move. Must have an engine to 
determine legal moves and this needs to be a part of it.
Also, pawn promotions are a special move.
*************************************************************************************/
using System.Collections.Generic;

public enum E_Side{NA, WHITE, BLACK}
public enum E_Piece{PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING}

public struct S_ChessMove
{
    // side? Maybe that's implied?
    E_Side              mSide;
    // piece, but is the piece a number (eg. white dark bishop == 3, white light bishop == 6)
    E_Piece             mPiece;
    // previous square
    public iVec2        ixPrevSq;
    // current square
    public iVec2        ixCurSq;
    // if there was a capture
    bool                mCapture;
    // maybe check or not?
    bool                mCheck;
    // maybe checkmate or not? -> Yes.
    bool                mMate;
}

public class TST_MoveList
{
    public List<S_ChessMove>            mMoves;
}
