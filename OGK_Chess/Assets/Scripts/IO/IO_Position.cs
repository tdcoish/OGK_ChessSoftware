/*************************************************************************************
Struct that stores a certain position on a chess board.
*************************************************************************************/
using UnityEngine;
using System.IO;

public enum TURN_OWNER{WHITE, BLACK}

public struct DT_Pos
{
    public DT_Pos(TURN_OWNER enTurn = TURN_OWNER.WHITE)
    {
        _turn = enTurn;
        _id = 0;            // get rid of, only used for giving pos name.
        _numPieces = 0;
    }

    public int              _id;
    public TURN_OWNER       _turn;
    public int              _numPieces;
    // also, the actual positions.
}


public static class IO_Position
{

    public static void FSavePos(DT_Pos d)
    {
        // need error corection.
        if(true){
            Debug.Log("Need error correction still");
        }

        Debug.Log("Saving Position...");
        StreamWriter sw = new StreamWriter(Application.dataPath+"/FILE_IO/Positions/"+d._id.ToString()+".txt");        
        sw.WriteLine("ID");
        sw.WriteLine(d._id);
        sw.WriteLine("TURN: ");
        sw.WriteLine(d._turn);       // curious how the enum is written.
        sw.WriteLine("Num Pieces");
        sw.WriteLine(5);

        sw.Close();
    }

    public static DT_Pos FLoadPos(string fileName)
    {
        DT_Pos p = new DT_Pos();

        string path = Application.dataPath+"FILE_IO/Positions/"+fileName+".txt";
        string[] sLines = System.IO.File.ReadAllLines(path);

        for(int i=0; i<sLines.Length; i++)
        {
            if(sLines[i].Contains("ID")){
                p._id = int.Parse(sLines[i+1]);
            }
            if(sLines[i].Contains("Num Pieces")){
                p._numPieces = int.Parse(sLines[i+1]);
            }
            if(sLines[i].Contains("TURN")){
                int temp = int.Parse(sLines[i+1]);
                if(temp == 0){
                    p._turn = TURN_OWNER.WHITE;
                }else{
                    p._turn = TURN_OWNER.BLACK;
                }
            }
        }

        return p;
    }
}
