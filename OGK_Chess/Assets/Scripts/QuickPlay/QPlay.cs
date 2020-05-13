/*************************************************************************************
Manager for the play scene
*************************************************************************************/
using UnityEngine;


// Game starts
// board set up
// Set pieces up
// Load in starting position (just set it up for now manually)
// Click on piece, get UI to show this.
// Can click again to move piece to wherever.
// Show this.

// Now, images are correct, game objects are correct
// However, we now need a way to switch them over to the new square.
// Let's make code that at least shows which piece you clicked on.

public class QPlay : MonoBehaviour
{
    public DT_BD                _board;

    void Start()
    {
        _board = new DT_BD();


    }

    void Update()
    {
        
    }



    public void SetUpBoard()
    {

    }
}
