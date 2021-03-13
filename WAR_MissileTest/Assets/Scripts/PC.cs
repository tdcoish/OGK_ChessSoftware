/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PC : MonoBehaviour
{

    public float _accRate = 0.1f;

    public float _maxSpd = 0.5f;

    public Vector3 mVel;

    void Start()
    {
        mVel = new Vector3();
    }

    void Update()
    {
        Vector2 acc = Vector2.zero;

        if(Input.GetKey(KeyCode.A)){
            acc.x -= _accRate;
        }
        if(Input.GetKey(KeyCode.D)){
            acc.x += _accRate;
        }
        if(Input.GetKey(KeyCode.W)){
            acc.y += _accRate;
        }
        if(Input.GetKey(KeyCode.S)){
            acc.y -= _accRate;
        }
        acc = Vector3.Normalize(acc); acc *= _accRate;

        acc *= Time.deltaTime;

        mVel += (Vector3)acc;
        if(mVel.magnitude > _maxSpd){
            mVel *= (_maxSpd/mVel.magnitude);
        }

        Vector3 pos = transform.position;
        pos += mVel * Time.deltaTime;
        transform.position = pos;
    }

}
