using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public Text deadText;
    //private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl playerControl;
    private GameObject player;
    private double health;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        health = player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().health;
        deadText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (health < 0)
        {
            deadText.enabled = true;
        }
	
	}
}
