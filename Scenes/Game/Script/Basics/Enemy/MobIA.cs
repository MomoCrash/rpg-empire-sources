using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobIA : MonoBehaviour
{

    [SerializeField] float ViewRange;
    [SerializeField] float Speed;
    [SerializeField] float MoveRangeX;
    [SerializeField] float MoveRangeY;

    [SerializeField] MobType Type;
    [SerializeField] int Damage;

    private GameObject _nearestPlayer;

    private bool IsMoveX;
    private bool IsMoveY;
    private float MoveToX;
    private float MoveToY;


    // Start is called before the first frame update
    void Start()
    {


        
    }

    void FixedUpdate() 
    {

        var moveRandomizer = Random.value;

        if (IsMoveX || IsMoveY) 
        {

            if ( gameObject.transform.position.x == MoveToX ) {
                IsMoveX = false;
            } else {
                gameObject.transform.Translate(Speed * Time.deltaTime, 0, 0);
            }
            if ( gameObject.transform.position.y == MoveToY ) {
                IsMoveY = false;
            } else {
                gameObject.transform.Translate(0, Speed * Time.deltaTime, 0);
            }

            if (0.2 < moveRandomizer && moveRandomizer < 0.3 ) {
                IsMoveX = false;
                IsMoveY = false;
            }

        } else if ( moveRandomizer <= 0.5 && !IsMoveX && !IsMoveY) {
            MoveToX = gameObject.transform.position.x + Random.Range(-MoveRangeX, MoveRangeX);
            MoveToY = gameObject.transform.position.y + Random.Range(-MoveRangeY, MoveRangeY);
        }

    }

}


public enum MobType 
{

    PASISVE,
    NEUTRAL,
    HOSTILE

}