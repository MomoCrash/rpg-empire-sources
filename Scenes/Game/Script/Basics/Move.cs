using UnityEngine;
using System;

public class Move : MonoBehaviour
{

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

    private Animator animator;

    private DateTime _lastSprintLost;
    private bool _canSprint = true;
    private bool _canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
        animator = player.GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        float sprint = (Input.GetAxis("Sprint") + 1) * sprintSpeed;
        if (!_canSprint)
        {
            sprint = 1;
        }
        TimeSpan elapsedTime = (DateTime.Now - _lastSprintLost);

        player.transform.Translate(0, vert * (speed - Math.Abs(horz * 3)) * sprint * Time.deltaTime, 0);
        player.transform.Translate(horz * (speed - Math.Abs(vert * 3)) * sprint * Time.deltaTime, 0, 0);

        cameraPosition = targetObject.position + initalOffset;
        transform.position = Vector2.Lerp(transform.position, cameraPosition, smoothness * Time.fixedDeltaTime);

        if (Input.GetAxis("Sprint") > 0 
            && elapsedTime.TotalMilliseconds > sprintLostInterval 
            && player.GetComponent<PlayerManager>().Inventory.HasEnoughEnergy(1))
        {
            _lastSprintLost = DateTime.Now;
            player.GetComponent<PlayerManager>().Inventory.UseEnergy(1);
            _canSprint = true;
        } else if (!player.GetComponent<PlayerManager>().Inventory.HasEnoughEnergy(1))
        {
            _canSprint = false;
        }
        

        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }

        if (animator != null)
        {

            if (Input.GetAxis("Horizontal") < 0)
            {
                animator.SetInteger("direction", 0);
                return;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                animator.SetInteger("direction", 1);
                return;
            }

        }

    }
}
