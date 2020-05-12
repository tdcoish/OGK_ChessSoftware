/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MN_Main : MonoBehaviour
{
    
    public void BT_QuickPlay()
    {
        SceneManager.LoadScene("QuickPlay");
    }
    public void BT_Credits()
    {
        Debug.Log("Here");
        SceneManager.LoadScene("Credits");
    }
    public void BT_Quit()
    {
        Application.Quit();
    }
}
