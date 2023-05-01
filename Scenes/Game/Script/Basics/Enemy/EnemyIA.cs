using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{

    [SerializeField] float ViewRange;
    [SerializeField] float Speed;
    [SerializeField] float MoveRangeX;
    [SerializeField] float MoveRangeY;

    [SerializeField] int Damage;

    private GameObject _nearestPlayer;
    private Vector3 _nextRandomPosition;

    void FixedUpdate()
    {

        return;
       
        if (HasNearPlayer(gameObject.transform.position, ViewRange))
        {

            gameObject.transform.localPosition = Vector2.Lerp(gameObject.transform.position, _nearestPlayer.transform.position, Speed);
            _nextRandomPosition = Vector3.zero;

        } else
        {

            if (_nextRandomPosition == Vector3.zero)
            {

                _nextRandomPosition = new(gameObject.transform.position.x + Random.Range(-MoveRangeX, MoveRangeX), gameObject.transform.position.y + Random.Range(-MoveRangeY, MoveRangeY));

            } else
            {

                if (Vector3.Distance(_nextRandomPosition, gameObject.transform.localPosition) <= 3)
                {
                    _nextRandomPosition = Vector3.zero;
                } else
                {
                    gameObject.transform.localPosition = Vector2.Lerp(gameObject.transform.position, _nextRandomPosition, Speed);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        print(collision);

        if (collidedObject.CompareTag("Player"))
        {
            _nearestPlayer.GetComponent<PlayerManager>().Inventory.Damage(Damage);
        }
        else
        {
            _nextRandomPosition = new(gameObject.transform.position.x + Random.Range(-MoveRangeX, MoveRangeX), gameObject.transform.position.y + Random.Range(-MoveRangeY, MoveRangeY));
        }
    }

    public bool HasNearPlayer(Vector3 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Player"))
            {
                _nearestPlayer = hitCollider.gameObject;
                return true;
            }

        }
        return false;
    }

}
