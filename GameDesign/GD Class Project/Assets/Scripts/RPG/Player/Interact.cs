using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    #region RayCasting Info
    //RAY - A ray is an infinite line starting at an origin and going in some direction.
    //RAYCASSTING - Casts a ray, from point origin, in direction, for length maxDistance,
    //against all colliderns in the scene
    //RAYCASTHIT - Structure used to get information back from a raycast
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //create the ray
            Ray interact;
            //assign origin
            interact = Camera.main.ScreenPointToRay
                (new Vector2(Screen.width/2, Screen.height/2));
            //hit info
            RaycastHit hitInfo;
            //Cast line
            if (Physics.Raycast(interact, out hitInfo, 10))
            {
                #region NPC
                if(hitInfo.collider.tag == "NPC")
                {
                    Debug.Log("NPC");
                    //if (hitInfo.collider.GetComponent<Dialogue>())
                    //{
                    //    hitInfo.collider.GetComponent<Dialogue>().showDlg = true;
                    //    GameManager.gamePlayState = GamePlayState.MenuPause;
                    //}
                }
                #endregion
                #region Item
                GameObject temp = hitInfo.collider.gameObject;
                if (hitInfo.collider.tag == "Item" && temp.GetComponent<ItemHandler>() != null)
                {
                    Debug.Log("Item");
                    temp.GetComponent<ItemHandler>().OnCollection();
                }
                #endregion
                #region Chest
                if (hitInfo.collider.tag == "Chest")
                {
                    Debug.Log("Chest");
                }
                #endregion
            }
        }
    }
}
