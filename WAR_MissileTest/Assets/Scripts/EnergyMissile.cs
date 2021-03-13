/*************************************************************************************
This is the missile that actually somewhat obeys real physics.

Update, I'll have it able to pull higher turn rate at lower speeds, decreasing linearly
down to max speed.
*************************************************************************************/
using UnityEngine;

public class EnergyMissile : MonoBehaviour
{
    public PC               refPC;
    public float            SPEED_OFF_RAIL = 0.5f;
    public float            BURN_TIME = 10f;
    public float            burnTimeLeft;
    public float            MAX_SPEED = 1f;
    // all numbers in km, not meters
    public float            ACC_UNDER_POWER = 0.100f;      // 10g == 100m/s.
    
    // drag has factors. 1. exponentially higher at max speed.
    // 2. exponentially higher when turning.
    // 3. also T/W ratio for acc. let's say that it's 10, or 100m/s.
    // 4. At max spd, drag == thrust, so drag is 100m/s2 at max speed, but only 1/4 that at lower speeds.
    // 5. To fake turn drag, just say that every 1 degree turn adds another 10m/s drag, play with it later.
    public float            EXTRA_DRAG_PER_DEGREE_TURN = 0.01f;        // in g, so 10m/s2.
    public float            MAX_TURN_RATE_ANGLE = 4.0f;
    private float           MIN_TURN_RATE_ANGLE;
    public float            MAX_TURN_RATE_NO_POWER = 0.5f;

    public float            mDistanceTraveled = 0f;

    private TrailRenderer   cTrail;

    public CalcIntSpot      PF_CalcIntSpot;
    public CalcActSpot      PF_CalcActSpot;

    private Vector3         mVel;

    void Start()
    {
        cTrail = GetComponentInChildren<TrailRenderer>();
        if(!cTrail){
            Debug.Log("Couldn't get trail");
        }

        MIN_TURN_RATE_ANGLE = MAX_TURN_RATE_ANGLE * 4f;

        refPC = FindObjectOfType<PC>();
        mVel = new Vector3();

        mVel = refPC.transform.position - transform.position;
        mVel = Vector3.Normalize(mVel);
        // mVel = Vector3.down;
        mVel *= SPEED_OFF_RAIL;

        burnTimeLeft = BURN_TIME;
    }

    void Update()
    {

        // WITH INTERCEPT LEAD - Figure out how long it will take to hit them, where thye will be, then adjust accordingly.
        float timeToHit = Vector3.Distance(transform.position, refPC.transform.position) / mVel.magnitude;
        // should actually be the difference in magnitude.
        Vector3 velDif = mVel - refPC.mVel;
        timeToHit = Vector3.Distance(transform.position, refPC.transform.position) / velDif.magnitude;      // SUCCESS!
        Debug.Log("Time to hit: " + timeToHit);
        // as a better rule, I'm taking an imagined current speed halfway between the current speed and the max speed.
        float timeToHitAtMaxSpd = Vector3.Distance(transform.position, refPC.transform.position) / MAX_SPEED;
        float avgTimeToHit = (timeToHit+timeToHitAtMaxSpd)/2;
        // hmmm. As we get closer, we need to use the real time to hit more and more. - actually we only need max spd hack when really far away
        // so we should be weighting the true course very strongly.
        float timeWeight = 30f;
        float minTimeToUseMaxSpd = 1f;
        if(timeToHit < minTimeToUseMaxSpd){
            avgTimeToHit = timeToHit;
        }else if(timeToHit > timeWeight){
            avgTimeToHit = timeToHitAtMaxSpd;
        }else{
            // gonna avg this out even more to make it smoother.
            float percentThere = (timeToHit-minTimeToUseMaxSpd)/(timeWeight - minTimeToUseMaxSpd);  // so at 15 seconds to impact, split the dif.
            avgTimeToHit = timeToHit*(1-percentThere) + timeToHitAtMaxSpd*percentThere;
        }

        Debug.Log("Averaged Time: " + avgTimeToHit);
        Vector3 interceptSpot = refPC.transform.position + refPC.mVel*avgTimeToHit;


        // show where we're trying to get to.
        Instantiate(PF_CalcIntSpot, interceptSpot, transform.rotation);
        // show where we're currently getting to.
        Vector3 curProjSpot = transform.position + mVel*timeToHit;
        Instantiate(PF_CalcActSpot, curProjSpot, transform.rotation);

        Vector3 leadVel = interceptSpot - transform.position;
        leadVel = Vector3.Normalize(leadVel);
        leadVel *= mVel.magnitude;   

        float sumAccMinusDrag = 0f;

        // make that trail white under no power.
        if(burnTimeLeft < 0f){
            cTrail.startColor = Color.white;
        }

        // alright here's where we differ. We figure out what our new heading is, but then we use the dif to subtract drag.
        Vector3 newHeading = new Vector3();
        // percent of our max speed in between min and max rate.
        float curMaxTurnRate = Mathf.Lerp(MIN_TURN_RATE_ANGLE, MAX_TURN_RATE_ANGLE, (mVel.magnitude / MAX_SPEED));
        if(burnTimeLeft > 0f){
            newHeading = Vector3.RotateTowards(mVel.normalized, leadVel.normalized, (Mathf.Deg2Rad* curMaxTurnRate)*Time.deltaTime, 0f); 
        }else{
            newHeading = Vector3.RotateTowards(mVel.normalized, leadVel.normalized, (Mathf.Deg2Rad* MAX_TURN_RATE_NO_POWER)*Time.deltaTime, 0f); 
        }
        float angleDif = Vector3.Angle(mVel.normalized, newHeading.normalized);
        mVel = newHeading.normalized * mVel.magnitude;

        // first, propulsion forwards.
        if(burnTimeLeft > 0f){
            sumAccMinusDrag += ACC_UNDER_POWER * 1000f;
        }
        float dragFromTurn = Mathf.Abs(angleDif) * EXTRA_DRAG_PER_DEGREE_TURN * 10000f;
        Debug.Log("Extra induced drag from turning: " + dragFromTurn);
        float percentMaxSpd = (mVel.magnitude/MAX_SPEED);
        dragFromTurn *= percentMaxSpd*percentMaxSpd;        // so at half speed, quarter drag.
        sumAccMinusDrag -= dragFromTurn;     // extra drag from turning.
        // you know, that would actually be higher at speed.
        Debug.Log(mVel.magnitude);
        float levelFlightDrag = percentMaxSpd * percentMaxSpd * ACC_UNDER_POWER * 1000f;
        Debug.Log("Drag induced from level flight: " + levelFlightDrag);
        sumAccMinusDrag -= levelFlightDrag;

        Debug.Log("Net Acceleration per s in m/s: " + sumAccMinusDrag);
        sumAccMinusDrag /= 1000f;
        burnTimeLeft -= Time.deltaTime;
        Debug.Log("Burn time left: " + burnTimeLeft);
        
        mVel += (mVel.normalized) * sumAccMinusDrag * Time.deltaTime;

        Vector3 pos = transform.position;
        pos += mVel * Time.deltaTime;
        transform.position = pos;

        Debug.Log("Speed in kmph: " + mVel.magnitude * 3600);

        mDistanceTraveled += mVel.magnitude * Time.deltaTime;
        Debug.Log("Distance Traveled in km: " + mDistanceTraveled);


        // rotate facing velocity.
        transform.rotation = Quaternion.Euler(mVel);

        // If the player has gotten behind us, then just destroy ourselves.
        if(burnTimeLeft < 0f){
            if(Vector3.Dot(mVel, (refPC.transform.position - transform.position)) < 0f)
            {
                Debug.Log(mVel);
                if(Vector3.Distance(transform.position, refPC.transform.position) > 4f){
                    Debug.Log("Player defeated missile");
                    Destroy(gameObject);
                }
            }

        }
    }
}
