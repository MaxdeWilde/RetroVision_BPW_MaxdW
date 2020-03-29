using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Image imageDashCooldown;
    public Image imageDashAnimation;
    public Image imageRunAnimation;
    
    public Transform manaCounter;
    public Transform maxCounter;

    public string PlayerStatus;
    private string prevStatus;
    private float Mana;
    public float maxMana = 3;

    public float DashCD = 0.8f;
    private float NextDash;
    public float dashDuration = 0.3f;
    private float dashCurrentDuration;
    public float JumpCD = 0.3f;
    private float NextJump;
    public float wallCounter = 3f;

    public float moveSpeed = 10f;
    public float groundAccel = 0f;
    public float gravity = -60f;
    public float drag = 0.9f;
    public float jumpHeight = 7f;
    public float dashDist = 70f;
    public float airAccel = 0;
    public float dA = 30f;
    public float maxAccelair = 8f;
    private float checkSpeed = 0;

    public Transform groundCheck;
    public Transform wallCheck;
    private float groundDistance = 0.01f;
    private float wallDistance = 0.65f;
    public LayerMask groundMask;

    public Vector3 moveDir;
    public Vector3 velocity;
    public Vector3 airSpeed;
    public Vector3 onlyDown;
    public bool Dashing = false;
    bool dashOnCooldown = false;
    public float increaseFV = 0f;

    void Awake()
    {
        imageDashAnimation.gameObject.SetActive(false);
        imageRunAnimation.gameObject.SetActive(false);
    }

    void Start()
    {
        imageDashAnimation = imageDashAnimation.GetComponent<Image>();
        imageRunAnimation = imageRunAnimation.GetComponent<Image>();
    }

    void Update()
    {
        //---Display
        manaCounter.GetComponent<Text>().text = Mana.ToString();
        maxCounter.GetComponent<Text>().text = "/ " + maxMana.ToString();

        if (Mana == 0)
        {
            manaCounter.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
            maxCounter.GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            manaCounter.GetComponent<Text>().color = new Color(0.0f, 1.0f, 0.84f);
            maxCounter.GetComponent<Text>().color = new Color(0.0f, 1.0f, 0.84f);
        }

        Debug.Log(PlayerStatus + " Dashing: " + Dashing + " Accel: " + airSpeed.magnitude + " Normal: " + moveDir.magnitude);
        
        if (prevStatus != PlayerStatus)
        {
            prevStatus = PlayerStatus;
        }

        //Camera
        Camera.main.fieldOfView = 60f + increaseFV;

        //---Movement

        //Direction
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        moveDir = z * transform.forward + x * transform.right;
        moveDir = Vector3.Normalize(moveDir);

        //Gravity and drag
        velocity.y += gravity * Time.deltaTime;
        velocity.y /= 1 + drag * Time.deltaTime;
        velocity.x /= 1 + drag * Time.deltaTime;
        velocity.z /= 1 + drag * Time.deltaTime;

        //Move
        if (PlayerStatus != "Wall")
        {
            controller.Move(velocity * Time.deltaTime);

        }

        //Sprint Animation
        if (Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.z, 2)) > 18)
        {
            
            if (checkSpeed < 30)
            {
                checkSpeed++;
            }

            if (Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.z, 2)) > 18 && checkSpeed >= 30)
            {
                imageRunAnimation.gameObject.SetActive(true);
                if (increaseFV < 10f)
                {
                    increaseFV += 0.6f;
                }
            }
            
            
        }
        else
        {
            imageRunAnimation.gameObject.SetActive(false);
            checkSpeed = 0;

            if (Time.time > dashCurrentDuration && increaseFV > 0)
            {
                increaseFV -= 0.7f;
            }
            if (increaseFV < 0)
            {
                increaseFV = 0f;
            }
        }

        //Groundcheck
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            PlayerStatus = "Grounded";
            Mana = maxMana;
        }
        else
        {
            PlayerStatus = "Airborne";
        }

        //Wallcheck
        if (Physics.CheckSphere(wallCheck.position, wallDistance, groundMask))
        {
            Debug.Log("thats a spicy wall");
           
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (wallCounter > 0)
                {
                    PlayerStatus = "Wall";
                    wallCounter -= 1 * Time.deltaTime;
                }
                else
                {
                    wallCounter = 0;
                }
            }  
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                wallCounter = 0;
            }
        }
        else
        {
            wallCounter = 3f;
        }

        //Grounded
        if (PlayerStatus == "Grounded")
        {
            velocity.x = moveDir.x * (moveSpeed + groundAccel);
            velocity.z = moveDir.z * (moveSpeed + groundAccel);
            airAccel= 0;

            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (moveDir != Vector3.zero)
            {
                if (moveSpeed + groundAccel < 20)
                {
                    groundAccel += 0.1f;
                }

            }
            else
            {
                groundAccel = 0;
            }
        }

        //Airborne
        if (PlayerStatus == "Airborne")
        {
            airSpeed = moveDir * airAccel;

            Color manacolor = manaCounter.GetComponent<Text>().color;
            manacolor.a = 1;
            manaCounter.GetComponent<Text>().color = manacolor;

            Color maxcolor = manaCounter.GetComponent<Text>().color;
            maxcolor.a = 1;
            maxCounter.GetComponent<Text>().color = maxcolor;

            if (moveDir != Vector3.zero)
            {
                if (airSpeed != Vector3.zero && Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.z, 2)) < moveSpeed + groundAccel)
                {
                    velocity.x += airSpeed.x;
                    velocity.z += airSpeed.z;
                }
                
                if (airAccel < maxAccelair)
                {
                    airAccel += dA * Time.deltaTime;
                }
                else
                {
                    airAccel = maxAccelair;
                }
            }
            else
            {
                airAccel = 0;
                groundAccel = 0;
            }
        } 
        else
        {
            Color manacolor = manaCounter.GetComponent<Text>().color;
            manacolor.a = 0.4f;
            manaCounter.GetComponent<Text>().color = manacolor;

            Color maxcolor = manaCounter.GetComponent<Text>().color;
            maxcolor.a = 0.4f;
            maxCounter.GetComponent<Text>().color = manacolor;
        }

        if (PlayerStatus == "Wall")
        {
            onlyDown = new Vector3(0.0f, velocity.y, 0.0f);
            controller.Move(onlyDown * Time.deltaTime);
            velocity.x = moveDir.x * moveSpeed;
            velocity.z = moveDir.z * moveSpeed;
            airAccel = 0;

            if (velocity.y < 0)
            {
                velocity.y = -1f;
            }

            if (prevStatus != "Wall" && Mana < maxMana)
            {
                Mana++;
            }
        }

        //--- Abilities

        //Extra jump
        if (Input.GetButtonDown("Jump"))
        {
            if (PlayerStatus == "Grounded")
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (PlayerStatus == "Airborne" && Mana > 0 && Time.time > NextJump)
            {
                NextJump = Time.time + JumpCD;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                Mana--;
            }
            if (PlayerStatus == "Wall")
            {
                wallCounter = 0f;
                PlayerStatus = "Airborne";
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        //Aerial dash
        if (Input.GetKey(KeyCode.LeftShift) && PlayerStatus == "Airborne" && Mana > 1 && Time.time > NextDash)
        {
            NextDash = Time.time + DashCD;
            dashCurrentDuration = Time.time + dashDuration;
            Mana -= 2;
            imageDashCooldown.fillAmount = 1;
            dashOnCooldown = true;
        }

        if (Time.time < dashCurrentDuration)
        {
            Dashing = true;
            velocity = Vector3.zero;
            imageDashAnimation.gameObject.SetActive(true);
            controller.Move(Camera.main.transform.forward * dashDist * Time.deltaTime);

            if (Time.time < dashCurrentDuration - 2*(dashDuration/3))
            {
                increaseFV += 1f;
            }
            if (Time.time > dashCurrentDuration - (dashDuration/3))
            {
                increaseFV -= 1f;
            }
        }

        if (Time.time > dashCurrentDuration)
        {
            Dashing = false;
            imageDashAnimation.gameObject.SetActive(false);
        }

        if (dashOnCooldown)
        {
            imageDashCooldown.fillAmount -= 1 / DashCD * Time.deltaTime;

            if (imageDashCooldown.fillAmount == 0)
            {
                dashOnCooldown = false;
            }
        }


    }

}