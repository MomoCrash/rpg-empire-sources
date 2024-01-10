using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{

    private void FixedUpdate()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.position.x)
        {
            GetComponent<Animator>().SetInteger("direction", 1);
        } else
        {
            GetComponent<Animator>().SetInteger("direction", 0);
        }
        
        var move = Vector2.Lerp(transform.position, mousePos, 0.5f * Time.fixedDeltaTime);
        if (move.x < 0.4f || move.y < 0.4f)
        {
            GetComponent<Animator>().SetBool("move", false);
        } else
        {
            GetComponent<Animator>().SetBool("move", true);
        }
        transform.position = move;

    }

}
