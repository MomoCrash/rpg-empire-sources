using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public static Dictionary<GameObject, Vector2> inTransition = new();
    private float smooth = 1f;

    private static List<Sprite> textures = new();

    void Start()
    {

        foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites"))
        {
            textures.Add(sprite);
        }

    }

    void FixedUpdate()
    {

        foreach (KeyValuePair<GameObject, Vector2> itemDef in inTransition)
        {
            if (itemDef.Key == null)
            {
                inTransition.Remove(itemDef.Key);
                break;
            }
            itemDef.Key.transform.localPosition = Vector2.Lerp(itemDef.Key.transform.position, itemDef.Value, smooth * Time.fixedDeltaTime);

            if (Vector2.Distance(itemDef.Key.transform.localPosition, itemDef.Value) < 1 || itemDef.Key.CompareTag("Pickuped"))
            {

                inTransition.Remove(itemDef.Key);
                break;

            }
        }
    }

    public static GameObject DropItem(Vector2 position, Item toDrop)
    {

        GameObject newDrop = new GameObject(toDrop.UniqueId.ToString());
        newDrop.tag = "Item";
        newDrop.transform.parent = GameObject.Find("Items").transform;
        newDrop.transform.localPosition = position;
        newDrop.transform.localScale = new Vector3(2.7f, 2.7f, 0);
        newDrop.AddComponent<SpriteRenderer>();
        newDrop.AddComponent<BoxCollider2D>();
        BoxCollider2D boxCollider2D = newDrop.GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(0.26f, 0.26f);
        boxCollider2D.isTrigger = true;
        SpriteRenderer spriteRenderer = newDrop.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = textures[toDrop.TextureIndex];
        spriteRenderer.sortingLayerName = "Player";

        return newDrop;

    }

    public static GameObject DropItemWithTransision(Vector2 start, Vector2 end, Item toDrop)
    {

        GameObject newDrop = DropItem(start, toDrop);

        inTransition.Add(newDrop, end);
        return newDrop;

    }

    public static void DropItemStackInRange(Vector3 center, ItemStack toDrop, float x, float xMax, float y, float yMax)
    {

        while (toDrop.Remove(1))
        {

            var direction = new Vector3(Random.Range(x, xMax), Random.Range(y, yMax), 0);
            DropItemWithTransision(center, center + direction, toDrop.Item);

        }

    }

    public static Sprite GetMaterialSprite(int textureIndex)
    {
        return textures[textureIndex];
    }

}
