using System.Collections;
using System.Linq;
using UnityEngine;

public class Ressources : MonoBehaviour
{

    [Header("Ressources parameters")]
    [SerializeField] [Range(1, 2000)] int maxRessourceUse;

    [SerializeField] Material[] Drops;
    [SerializeField] int[] MaxAmount;

    [SerializeField] [Range(1, 200)] int CollectCost = 5;
    [SerializeField] [Range(1, 60)] float Duration = 1;

    public int RessourceType;

    private Item[] _dropableItems;
    private bool _canEnable = false;
    private GameObject _currentUser;
    private PlayerManager _currentUserManager;

    private bool _isUsed = false;
    private bool _isLeftClicked = false;
    private int _leftUse;
    private int id;

    private void Start()
    {

        _leftUse = Random.Range(1, maxRessourceUse);
        _dropableItems = new Item[Drops.Length];

        int i = 0;
        foreach (Material material in Drops)
        {

            _dropableItems[i] = Item.GetItem(material);
            i++;

        }

        id = gameObject.GetInstanceID();

    }

    private void Update()
    {
        
        if (_canEnable && Input.GetMouseButtonDown(0) && _currentUser is not null && !_isUsed)
        {
            _currentUserManager.CurrentRessource = gameObject.GetInstanceID();
            StartCoroutine(DelayRessourcesCollect());
        }

        if (_isUsed && Input.GetMouseButtonDown(0))
        {
            _isLeftClicked = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            _canEnable = true;
            _currentUser = collision.gameObject;
            _currentUserManager = _currentUser.GetComponent<PlayerManager>();
            _currentUserManager.Inventory.ActionProgress = 0;
            _currentUser.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isUsed = false;
            _canEnable = false;
            collision.GetComponent<PlayerManager>().Inventory.ActionProgress = 0;
            collision.GetComponent<Animator>().SetBool("used", false);
            collision.transform.GetChild(0).gameObject.SetActive(false);
            _currentUserManager = null;
            _currentUser = null;
        }
    }

    IEnumerator DelayRessourcesCollect()
    {
        var inventory = _currentUserManager.Inventory;
        if (_currentUserManager.CurrentRessource != id) yield return null;
        if (inventory == null) yield return null;
        var animator = _currentUser.GetComponent<Animator>();
        while (_canEnable && inventory.UseEnergy(CollectCost))
        {
            _isUsed = true;
            foreach (int _ in Enumerable.Range(1, 10))
            {
                if (!_canEnable)
                {
                    _isUsed = false;
                    inventory.ActionProgress = 0;
                    animator.SetBool("used", false);
                    _currentUser.transform.GetChild(0).gameObject.SetActive(false);
                    yield return null;
                } else if (_isLeftClicked)
                {

                    _isLeftClicked = false;
                    inventory.ActionProgress += 1;
                    animator.SetBool("used", true);
                    continue;

                }
                else
                {
                    inventory.ActionProgress += 1;
                    animator.SetBool("used", true);
                    yield return new WaitForSeconds(Duration / 10);
                }
            }
            if (_canEnable)
            {
                _leftUse -= 1;
                inventory.ActionProgress = 0;

                foreach (int i in Enumerable.Range(0, _dropableItems.Length))
                {

                    ItemStack stack = new(_dropableItems[i], Random.Range(0, MaxAmount[i]));
                    ItemManager.DropItemStackInRange(gameObject.transform.position, stack, -1f, 1.3f, -2.6f, 10f);
                    _currentUser.GetComponent<QuestManager>().BreakRessource(RessourceType);

                }

                if (_leftUse == 0)
                {
                    animator.SetBool("used", false);
                    transform.SendMessageUpwards("RemoveRessourceData", gameObject.transform.localPosition);
                    Destroy(gameObject);
                }

            }

        }
        animator.SetBool("used", false);
        _isUsed = false;
        inventory.ActionProgress = 0;
        animator.SetBool("used", false);
    }

}
