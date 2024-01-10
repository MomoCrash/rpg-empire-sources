using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{

    public PlayerManager Player;

    private readonly List<GameObject> followingItems = new();
    public List<GameObject> InRangeMobs ;

    private void FixedUpdate()
    {

        foreach (var followingItem in followingItems)
        {

            followingItem.transform.localPosition = Vector2.Lerp(followingItem.transform.position, transform.position, 4f * Time.fixedDeltaTime);

            var distance = Vector2.Distance(Player.gameObject.transform.position, followingItem.gameObject.transform.position);

            if (distance < 0.2f)
            {

                var itemId = int.Parse(followingItem.name);
                Player.Inventory.AddItem(new ItemStack(Item.GetItem(itemId), 1));
                GameObject.Destroy(followingItem);
                ItemManager.inTransition.Remove(followingItem);
                followingItems.Remove(followingItem);
                Player.FastTab.SendFastTabMessage(new FastTab(new ItemStack(Item.GetItem(itemId), 1), 3, DateTime.Now));
                break;

            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Mobs"))
        {

            InRangeMobs.Add(collision.gameObject);

        }

        if (collision.gameObject.CompareTag("Item"))
        {

            followingItems.Add(collision.gameObject);
            collision.gameObject.tag = "Pickuped";

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Mobs"))
        {

            InRangeMobs.Remove(collision.gameObject);

        }

    }

}
