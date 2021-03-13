/*************************************************************************************
According to https://onlinemschool.com/math/assistance/vector/angl/
At 1500m/s if we can pull 10g's, then we can make about a 4.0846* turn.

Better way to do this would be simply controlling heading.
*************************************************************************************/
using UnityEngine;

public class Missile : MonoBehaviour
{
    public PC               refPC;
    public float            _moveSpd = 1f;
    public float            _maxTurngs = 10f; // g's per second. 1 = 10m/s
    public float            TURN_RATE_ANGLE = 4.0846f;

    public bool             _pullLead = false;

    public CalcIntSpot      PF_CalcIntSpot;
    public CalcActSpot      PF_CalcActSpot;

    private Vector3         mVel;

    void Start()
    {
        refPC = FindObjectOfType<PC>();
        mVel = new Vector3();

        mVel = refPC.transform.position - transform.position;
        mVel = Vector3.Normalize(mVel);
        // mVel = Vector3.down;
        mVel *= _moveSpd;
    }

    void Update()
    {
        // NO INTERCEPT LEAD - Try to acc straight to the target.
        if(!_pullLead)
        {
            Vector3 straightVel = refPC.transform.position - transform.position;
            straightVel = Vector3.Normalize(straightVel);
            straightVel *= _moveSpd;

            Vector3 newHeading = Vector3.RotateTowards(mVel, straightVel, (Mathf.Deg2Rad* TURN_RATE_ANGLE)*Time.deltaTime, 0f); 
            mVel = Vector3.Normalize(newHeading) * _moveSpd;
        }

        // WITH INTERCEPT LEAD - Figure out how long it will take to hit them, where thye will be, then adjust accordingly.
        if(_pullLead)
        {
            float timeToHit = Vector3.Distance(transform.position, refPC.transform.position) / _moveSpd;
            Debug.Log("Time to hit: " + timeToHit);
            Vector3 interceptSpot = refPC.transform.position + refPC.mVel*timeToHit;
            Debug.Log("Vel: " + refPC.mVel);    

            // show where we're trying to get to.
            Instantiate(PF_CalcIntSpot, interceptSpot, transform.rotation);
            // show where we're currently getting to.
            Vector3 curProjSpot = transform.position + mVel*timeToHit;
            Instantiate(PF_CalcActSpot, curProjSpot, transform.rotation);

            Vector3 leadVel = interceptSpot - transform.position;
            leadVel = Vector3.Normalize(leadVel);
            leadVel *= _moveSpd;   

            Vector3 newHeading = Vector3.RotateTowards(mVel, leadVel, (Mathf.Deg2Rad* TURN_RATE_ANGLE)*Time.deltaTime, 0f); 
            mVel = Vector3.Normalize(newHeading) * _moveSpd;
        }
        
        Vector3 pos = transform.position;
        pos += mVel * Time.deltaTime;
        transform.position = pos;


        // rotate facing velocity.
        transform.rotation = Quaternion.Euler(mVel);

        // If the player has gotten behind us, then just destroy ourselves.
        if(Vector3.Dot(mVel, (refPC.transform.position - transform.position)) < 0f)
        {
            if(Vector3.Distance(transform.position, refPC.transform.position) > 4f){
                Debug.Log("Player defeated missile");
                Destroy(gameObject);
            }
        }
    }
}
