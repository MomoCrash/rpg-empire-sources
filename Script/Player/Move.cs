using UnityEngine;
using System;

public class Move : MonoBehaviour
{

    public Joystick Joystick;

    [SerializeField] int speed = 10;
    [SerializeField] float sprintSpeed = 1.2f;
    [SerializeField] int sprintLostInterval;
    [SerializeField] GameObject cameraObject;
    [SerializeField] GameObject player;

    // Camera Follow
    [SerializeField] float smoothness;
    [SerializeField] Transform targetObject;

    private Vector3 initalOffset;
    private Vector3 cameraPosition;

    private Animator Animator;
    private AudioSource PlayerAudio;

    private DateTime _lastSprintLost;
    private bool _canSprint = true;
    readonly bool _canMove = true;

    private float lockSprint = .0f;

    // Start is called before the first frame update
    void Start()
    {
        
        Animator = player.GetComponent<Animator>();
        PlayerAudio = player.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        print(lockSprint);

        float vert = Joystick.Vertical;
        float horz = Joystick.Horizontal;
        float sprint = sprintSpeed;
        if (!_canSprint)
        {
            sprint = 1;
        } else if (lockSprint > .5f)
        {
            sprint = sprintSpeed;
        }
        TimeSpan elapsedTime = DateTime.Now - _lastSprintLost;

        player.transform.Translate(0, vert * (speed - Math.Abs(horz * 3)) * sprint * Time.deltaTime, 0);
        player.transform.Translate(horz * (speed - Math.Abs(vert * 3)) * sprint * Time.deltaTime, 0, 0);

        cameraPosition = targetObject.position + initalOffset;
        transform.position = Vector2.Lerp(transform.position, cameraPosition, smoothness * Time.fixedDeltaTime);

        if (lockSprint > .5f
            && elapsedTime.TotalMilliseconds > sprintLostInterval 
            && player.GetComponent<PlayerManager>().Inventory.HasEnoughEnergy(1))
        {
            Animator.SetBool("sprint", true);
            _lastSprintLost = DateTime.Now;
            player.GetComponent<PlayerManager>().Inventory.UseEnergy(1);
            _canSprint = true;
        } else if (!player.GetComponent<PlayerManager>().Inventory.HasEnoughEnergy(1))
        {
            _canSprint = false;
        }

        if (lockSprint == 0) {
            Animator.SetBool("sprint", false);
        }
        

        if (vert != 0 || horz != 0)
        {
            lockSprint += Time.fixedDeltaTime;
            Animator.SetBool("move", true);
            if (!PlayerAudio.isPlaying) {
                PlayerAudio.Play();
            }
        }
        else
        {
            lockSprint = 0;
            Animator.SetBool("move", false);
        }


        if (horz < 0)
        {
            Animator.SetInteger("direction", 0);
            return;
        }
        else if (horz > 0)
        {
            Animator.SetInteger("direction", 1);
            return;
        }

    }
}
