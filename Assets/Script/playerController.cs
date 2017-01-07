using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class playerController : MonoBehaviour
{
  
    public float speed;            // The speed that the player will move at.

    Vector3 movement;                   // The vector to store the direction of the player's movement.
    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.


    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 1000f;          // The length of the ray from the camera into the scene.

    public float maxTime = 30f;
    public float minTime = 20f;

    float eventtimer=0;
    float eventTime;

    float worktimer=0;
    float sleepytimer = 0;

    bool Tired;
    bool Sleepy;
   // bool working;


   
    AudioSource type;
    AudioSource yawn;


    public static int currentScore=0;
    public int workingspeed = 2;

    public Image workingImage;
    public Image TiredImage;
    public Image SleepyImage;

    public Slider ProgressBar;
    public Text Percentage;
    public Text WorkingHint;
    public Text StretchingHint;
    public Text SittingHint;
    public Text SleepHint;
    public Text QuittingHint;


    public Text TiredText;
    public Text SleepyText;
    public Text FineText;
    Color hide;
    Color show;

    public bool IsWalking;
    public bool IsNearChair;
    public bool IsNearBed;
    public bool IsSitting;
    public bool IsWorking;
    public bool IsTurning;

    //used for text hint
    bool sitted;
    bool worked;

    //retstart
    public Text congratulation;
    public Text startover;
    float restartDelay = 10f;
    float restarttimer;


    void Awake()
    {
        hide.a = 0;
        show.a = 255;

        AudioSource[] audios = GetComponents<AudioSource>();
        
        type = audios[1];
        yawn = audios[2];

        

        eventTime = Random.Range(minTime, maxTime);
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

        IsTurning = true;
    }


    void FixedUpdate()
    {

        if(currentScore>=100)
        {
            workingspeed = 0;
            Finished();
        }
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!Tired && !Sleepy)
            FineText.color = show;
        else
            FineText.color = hide;


        if (IsNearChair && !sitted)
        {
            SittingHint.color = show;
        }
        else
            SittingHint.color = hide;


        if(IsNearChair && sitted && !worked)
        {
            WorkingHint.color = show;
        }

        if(IsNearChair&&sitted&&worked)
        {
            QuittingHint.color = show;
        }

        if(IsNearBed)
        {
            SleepHint.color = show;
        }
        else
        {
            SleepHint.color = hide;
        }
        if (IsWorking && !Sleepy && !Tired)
        {
           
            eventtimer += Time.deltaTime;
            worktimer += Time.deltaTime;
            if (worktimer >= 3 )
            {
                currentScore += workingspeed;
                Percentage.text = currentScore + " %";
                ProgressBar.value = currentScore;
                worktimer = 0;
            }

            if (eventtimer >= eventTime)
            {
                type.Stop();
                yawn.Play();
                
                float temp = Random.Range(0, 10);
                if (temp <= 5f)
                {
                     anim.SetTrigger("Tired");
                     Tired = true;
                     TiredImage.color = show;
                    StretchingHint.color = show;
                    TiredText.color = show;

                }
                else
                {
                    anim.SetTrigger("Sleepy");
                    Sleepy = true;
                    workingImage.color = hide;
                    SleepyImage.color = show;
                    SleepyText.color = show;
                    IsWorking = false;
                    anim.SetBool("IsWorking", IsWorking);

                }

                eventtimer = 0;
            }
        }
        if(Tired &&IsSitting)
        {
            worktimer += Time.deltaTime;

            sleepytimer += Time.deltaTime;

            if(sleepytimer >=20)
            {
                yawn.Play();
                Tired = false;
                TiredImage.color = hide;
                TiredText.color = hide;
                StretchingHint.color = hide;

                anim.SetTrigger("Sleepy");
                Sleepy = true;
                workingImage.color = hide;
                SleepyImage.color = show;
                SleepyText.color = show;
                sleepytimer = 0;
                IsWorking = false;
               anim.SetBool("IsWorking", IsWorking);
            }
            if (worktimer >= 3)
            {
                currentScore += workingspeed/2;
                Percentage.text = currentScore + " %";
                ProgressBar.value = currentScore;
                worktimer = 0;
            }

        }
        if(Input.GetKeyDown(KeyCode.Z)&&IsNearChair)
        {
            IsTurning = false;
            IsSitting = true;
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(1,0,0));
            playerRigidbody.MoveRotation(newRotation);
            anim.SetBool("IsSitting",IsSitting);

            sitted = true;
            SittingHint.color = hide;
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsNearBed)
        {
            IsTurning = false;
            
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(1, 0, 0));
            playerRigidbody.MoveRotation(newRotation);
            anim.SetTrigger("Sleep");
            SleepyImage.color = hide;
            Sleepy = false;
            SleepyText.color = hide;

        }
        if(Input.GetKeyDown(KeyCode.X)&& !IsSitting && !IsNearBed && !IsNearChair )
        {
            IsTurning = false;

            anim.SetTrigger("Stretch");
            Tired = false;
            TiredImage.color = hide;
            TiredText.color = hide;
            StretchingHint.color = hide;
        }
        if (Input.GetKeyDown(KeyCode.W)&&IsSitting)
        {
            IsTurning = false;
            IsWorking = true;
            worked = true;

            anim.SetBool("IsWorking", IsWorking);

            WorkingHint.color = hide;

            type.Play();

            if(SleepyImage.color.a!=255)
             workingImage.color = show;

          //  working = true;
        }
        
        if(Input.GetKeyDown(KeyCode.Q)&&IsSitting)
        {
            IsWorking = false; 
            IsSitting = false;
            anim.SetBool("IsWorking", IsWorking);
            anim.SetBool("IsSitting", IsSitting);
            anim.SetBool("Quit",true);
            QuittingHint.color = hide;
            workingImage.color = hide;
          //  working = false;
            sitted = false;
            worked = false;
            WorkingHint.color = hide;
        }


        if (!IsWorking)
            type.Stop();
        if (Sleepy)
            type.Stop();
        
            // Move the player around the scene.



        if(IsTurning)
            Move(h, v);
              
        // Turn the player to face the mouse cursor.
        if(IsTurning)
          Turning();

        // Animate the player.
        if(IsTurning)
         Animating(h, v);


    }

    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
        //print(floorHit);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
         IsWalking = h != 0f || v != 0f;
      
        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", IsWalking);
       // print(walking);
    }

    public void setTurningAvailable()
    {
        IsTurning = true;

    }
    public void resumeType()
    {
        type.Play();
    }

    public void QuitFinished()
    {
        
        anim.SetBool("Quit", false);
    }
    void Finished()
    {
        type.Stop();
      
            if (restarttimer < restartDelay / 3 * 2)
            {

                congratulation.color = show;
                startover.color = hide;

            }
            else
            {
                congratulation.color = hide;
                startover.color = show;
            }


            restarttimer += Time.deltaTime;
            if (restarttimer >= restartDelay)
            {

            restarttimer = 0;
            currentScore = 0;
            startover.color = hide;
            congratulation.color = hide;
                SceneManager.LoadScene(0);

            }
        }
    
}