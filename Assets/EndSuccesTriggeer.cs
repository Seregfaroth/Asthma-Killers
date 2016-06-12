using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndSuccesTriggeer : MonoBehaviour {
    private bool wait;
    private double waitSec = 3;

    // Use this for initialization
    void Start () {
        wait = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (wait)
        {
            
            if ( waitSec > 0 )
            {
                waitSec -= Time.deltaTime;
            }
            if (waitSec <= 0)
                SceneManager.LoadScene("RoomOlavur");
        }
	}
    void OnTriggerEnter( Collider other )
    {
        Debug.Log("Enter: " + other.ToString());
        if (other.tag ==  "Player")
        {
            GetComponent<EscapeSucces>().succesText.enabled = true;
            wait = true;
        }
            
       
        

        
    }
}
