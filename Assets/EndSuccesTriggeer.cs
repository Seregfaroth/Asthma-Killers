using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndSuccesTriggeer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter( Collider other )
    {
        Debug.Log("Enter: " + other.ToString());
        if (other.tag ==  "Player") 
            GetComponent<EscapeSucces>().succesText.enabled = true;
        new WaitForSeconds(2);
        
        //SceneManager.LoadScene("RoomBo");
    }
}
