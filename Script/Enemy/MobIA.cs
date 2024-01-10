using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MobIA : MonoBehaviour
{

    [Header("Mob parameters")]
    public MobType Type;
    public float Strenght;
    public float MaxHealth;
    public float Health;
    public float Resistance;

    public float Speed;
    public float MoveRange;
    public float MoveTimeMin = 1.0f;
    public float MoveTimeMax = 3.0f;

    public LootTable Loots;
    public int dropNumber;

    [Header("Mob properties")]
    public MobState CurrentState;

    public Vector3 _moveVector = Vector3.zero;
    public float _speedMultiplier = 1.0f;
    public float _currentElapsedTime;
    public float _moveTime;
    public float _waitTime;

    public GameObject PlayerInRange;
    public ProgressBar HealthBar;

    void FixedUpdate() 
    {

        HealthBar.SetScale(MaxHealth, Health);

        _currentElapsedTime += Time.deltaTime;

        if (_waitTime < _currentElapsedTime && (_currentElapsedTime > _moveTime || _moveVector == Vector3.zero))
        {
            CalculateNextMove();
        }
        else
        {
            gameObject.transform.Translate(_moveVector * Time.deltaTime * Speed * _speedMultiplier);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") || collision.CompareTag("Mobs")) 
        {

            if (Type == MobType.PASISVE)
            {
                CurrentState = MobState.RUN_AWAY;
                PlayerInRange = collision.gameObject;
            }

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Mobs"))
        {
            CurrentState = MobState.WANDERING;
            PlayerInRange = null;
        }

    }

    public void Damage(float amount)
    {

        var appliableDamage = amount / Resistance;

        if (Health > appliableDamage)
        {
            Health -= appliableDamage / Resistance;
        } else
        {
            ItemStack[] mobLoots = Loots.GetDropItemStack(dropNumber);
            print(mobLoots.Length);
            foreach (ItemStack loot in mobLoots)
            {
                print(loot.Item.Name);
                ItemManager.DropItemStackInRange(gameObject.transform.position, loot, -1, 1, -1, 1);
            }
            GameObject.Destroy(gameObject);
        }
    }

    private void CalculateNextMove()
    {
        switch (CurrentState)
        {

            case MobState.WANDERING:
                {

                    _currentElapsedTime = 0;
                    _moveTime = Random.Range(MoveTimeMin, MoveTimeMax);
                    _waitTime = Random.Range(0.5f, 1.0f);
                    _speedMultiplier = 1.0f;

                    var xValue = (Random.Range(-MoveRange, MoveRange) + _moveVector.x * 2) / 3;
                    var yValue = (Random.Range(-MoveRange, MoveRange) + _moveVector.y * 2) / 3;

                    _moveVector = new Vector3(xValue, yValue, 0);

                    break;
                }

            case MobState.RUN_AWAY:
                {

                    _currentElapsedTime = 0;
                    _moveTime = MoveTimeMin;
                    _waitTime = 0.5f;
                    _speedMultiplier = 2f;

                    var mobPos = gameObject.transform.position;
                    var moveVector = -(PlayerInRange.transform.position - mobPos);

                    _moveVector = moveVector;

                    break;

                }

            default: break;

        }
    }

}

public enum MobType 
{

    PASISVE,
    NEUTRAL,
    HOSTILE

}

public enum MobState
{

    WANDERING,
    CHASING,
    RUN_AWAY,

}