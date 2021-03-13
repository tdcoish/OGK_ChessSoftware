/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public Missile PF_Missile;
    public EnergyMissile PF_EnergyMissile;
    public EnergyMissile PF_DogfightMissile;
    public EnergyMissile PF_InBetweenMissile;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // spawn a missile at our location.
            Instantiate(PF_DogfightMissile, transform.position, transform.rotation);
        }    

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Instantiate(PF_InBetweenMissile, transform.position, transform.rotation);
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt)){
            Instantiate(PF_EnergyMissile, transform.position, transform.rotation);
        }
    }
}
