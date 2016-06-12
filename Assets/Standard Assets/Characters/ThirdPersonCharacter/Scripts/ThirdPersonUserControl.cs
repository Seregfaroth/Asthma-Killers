using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public double health = 100;
        private double maxHealth = 100;
        private double minHealth = 0;
        public Text healthText;
        private bool healthChange;
        private AudioSource go;
        public AudioClip h60;
        public AudioClip h100;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private double breath;
        const double breathFreq = 6;
        public AudioClip soundHealth20;
        public AudioClip soundHealth40;
        public AudioClip soundHealth60;
        public AudioClip soundHealth80;
        public AudioClip soundHealth100;
        public AudioClip death;

        private void Start()
        {
            breath = breathFreq;
            //go = GetComponent<AudioSource>();
            setHealthText();

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            decreaseHealth(4*Time.deltaTime);

            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (breath < 0)
            {
                BreathSound();
                breath = breathFreq;
            }
            else
            {
                breath -= Time.deltaTime;
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            //setHealthText();
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }

        public void setHealth(double newHealth)
        {
            health = newHealth;
            setHealthText();
        }

        public void increaseHealth(double inc)
        {
            health = health + inc;
            if (health > maxHealth)
                health = maxHealth;
            setHealthText();
        }

        public void decreaseHealth(double dec)
        {
            health = health - dec;
            if (health < minHealth)
                health = minHealth;
            setHealthText();
        }

        public void setHealthText()
        {
            healthText.text = "Health: " + health.ToString();
        }

        public void BreathSound()
        {
            
            if (health > 80 && health <= 100)
            {
                GetComponent<AudioSource>().clip = soundHealth20;
            }
            else if (health > 60 && health <= 80)
            {
                GetComponent<AudioSource>().clip = soundHealth40;
            }
            else if (health > 40 && health <= 60)
            {
                GetComponent<AudioSource>().clip = soundHealth60;
            }
            else if (health > 20 && health <= 40)
            {
                GetComponent<AudioSource>().clip = soundHealth80;
            }
            else if (health > 0 && health <= 20)
            {
                GetComponent<AudioSource>().clip = soundHealth100;
            }
            else if (health < 0)
            {
                GetComponent<AudioSource>().clip = death;
            }
            GetComponent<AudioSource>().Play();
        }
    } }
