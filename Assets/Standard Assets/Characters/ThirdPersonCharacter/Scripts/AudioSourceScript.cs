using UnityEngine;
using System.Collections;

public class AudioSourceScript : MonoBehaviour {

    public AudioClip soundHealth60;
    public AudioClip soundHealth100;
    public AudioClip soundHealth20;
    public AudioClip soundHealth40;
    public AudioClip soundHealth80;
    public AudioClip death;
    public AudioSource s;
    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl t; 
    public GameObject go;
    //private int h = 

    // Use this for initialization
    void Start () {
        t = go.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>();
        s.clip = soundHealth20;
        s.Play();
        s.loop = true;       
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    public void ChangeHealthSound()
    {
        AudioClip oldClip = s.clip;
        s.loop = true;
        if (t.health > 80  && t.health <=100)
        {
            s.clip = soundHealth20;
        }
        else if (t.health > 60 && t.health <= 80)
        {
            s.clip = soundHealth40;
        }
        else if (t.health > 40 && t.health <= 60)
        {
            s.clip = soundHealth60;
        }
        else if (t.health > 20 && t.health <= 40)
        {
            s.clip = soundHealth80;
        }
        else if (t.health > 0 && t.health <= 20)
        {
            s.clip = soundHealth100;
        }
        else if (t.health < 0)
        {
            s.clip = death;
            s.loop = false;
        }

        if (oldClip!=s.clip) 
            s.Play();
        s.loop = true;
    }


}
