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

public class Board : MonoBehaviour
{
    public Square               PF_LightSquare;
    public Square               PF_DarkSquare;

    public DT_BD                _data;
    void Start()
    {
        _data = new DT_BD(8);

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
                    Instantiate(PF_LightSquare, pos, transform.rotation);
                }
                else
                {
                    // spawn a dark square here.
                    Instantiate(PF_DarkSquare, pos, transform.rotation);
                }
            }
        }
    }

    void Update()
    {
        
    }
}
