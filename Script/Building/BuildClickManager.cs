using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildClickManager : MonoBehaviour
{

    public PlayerManager Player;
    public BuildingMenu BuildingMenu;
    public InventoryMenu InventoryMenu;

    public AlchemistUpgrade Alchemist;

    public Animator BuildMenuAnimator;

    public TextMeshProUGUI BuildTitleField;
    public TextMeshProUGUI BuildDescField;
    public Image BuildImageField;

    private void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count < 1) {
                CloseMenu(false);
            } else
            {
                foreach (var result in results)
                {
                    if (result.gameObject.CompareTag("Controller"))
                    {
                        CloseMenu(true);
                    }
                }
            }

            var mouseClick = Input.GetTouch(0).position;
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseClick);

            var collider = Physics2D.OverlapBoxAll(mouseWorldPosition, new Vector2(2, 2), .0f);
           
            if (collider.Length > 0)
            {

                foreach (var collided in collider)
                {

                    if (collided.CompareTag("Alchemist"))
                    {

                        Alchemist.ToggleMenu();

                    }
                    
                    if (collided.CompareTag("Building"))
                    {

                        var buildManager = collided.gameObject.GetComponent<BuildManager>();

                        if (!BuildingMenu.IsOpen)
                        {
                            buildManager.EnableBuilding();
                            ShowMenu(buildManager);
                        }

                    }
                }
            }
        }
    }

    public void CloseMenu(bool all)
    {
        HideMenu();
        if (BuildingMenu.IsOpen || BuildingMenu.SelectedBuild != null)
        {
            BuildingMenu.CloseMenu();
            if (!all) return;
        }
        if (InventoryMenu.IsOpen)
        {
            InventoryMenu.CloseInventory();
            if (!all) return;
        }
    }

    public void ShowMenu(BuildManager build)
    {

        BuildMenuAnimator.SetBool("open", true);
        Player.CurrentBuilding = gameObject.transform.parent.gameObject;

        BuildTitleField.text = build.BuildTitle;
        BuildDescField.text = build.BuildDescription;

        BuildImageField.sprite = build.BuildIcon;

    }

    public void HideMenu()
    {
        BuildMenuAnimator.SetBool("open", false);
        Player.CurrentBuilding = null;
    }
}
