using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Player
{
    public class Inventory : MonoBehaviour
    {
        #region Variables

        // Items and our Inventory 
        public static List<Item> playerInv = new List<Item>();
        public Item selectedItem;
        public static bool  showInv;
        public static int   money;

        // Scrolling and Sorting
        public Vector2  scrollPos;
        public string   sortType = "All";
        public string[] enumTypesForItems = 
            {"Food","Weapon","Apparel","Crafting","Ingredient","Potion", "Scroll","Quest","Money" };

        // Equipment and Dropping
        public Transform dropLocation;
        [System.Serializable]
        public struct Equipment
        {
            public string slotName;
            public Transform equipmentLocation;
            public GameObject currentShop;
        }
        public Equipment[] equipmentSlots;

        // Other Inventories
        public static Chest.Inventory currentChest;
        public static Shop.Inventory currentShop;


        #endregion

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            playerInv.Add(ItemData.CreateItem(0));
            playerInv.Add(ItemData.CreateItem(1));
            playerInv.Add(ItemData.CreateItem(100));
            playerInv.Add(ItemData.CreateItem(101));
            playerInv.Add(ItemData.CreateItem(200));
            playerInv.Add(ItemData.CreateItem(201));
            playerInv.Add(ItemData.CreateItem(202));
            playerInv.Add(ItemData.CreateItem(300));
            playerInv.Add(ItemData.CreateItem(301));
            playerInv.Add(ItemData.CreateItem(500));
            playerInv.Add(ItemData.CreateItem(501));
            playerInv.Add(ItemData.CreateItem(100));
            playerInv.Add(ItemData.CreateItem(601));
#endif
        }

        // Update is called once per frame
        void Update()
        {
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                playerInv.Add(ItemData.CreateItem(0));
                playerInv.Add(ItemData.CreateItem(1));
                playerInv.Add(ItemData.CreateItem(100));
                playerInv.Add(ItemData.CreateItem(101));
                playerInv.Add(ItemData.CreateItem(200));
                playerInv.Add(ItemData.CreateItem(201));
                playerInv.Add(ItemData.CreateItem(202));
                playerInv.Add(ItemData.CreateItem(300));
                playerInv.Add(ItemData.CreateItem(301));
                playerInv.Add(ItemData.CreateItem(500));
                playerInv.Add(ItemData.CreateItem(501));
                playerInv.Add(ItemData.CreateItem(600));
                playerInv.Add(ItemData.CreateItem(601));
            }
        #endif

            // if inventory key is pressed
            // toggle both show inventory and game state 
            if (Input.GetKeyDown(KeyBinds.keys["Inventory"]))
            {
                showInv = !showInv; // opposite of what it was
                GameManager.gamePlayStates = showInv ? GamePlayStates.MenuPause : GamePlayStates.Game;
            }
        }

        void Display()
        {
            // store GameManager.scr values in a Vector3 so we don't have to retype GameManager.scr.x etc all the time
            Vector3 screenSize = GameManager.scr;

            // if we want to display everything in our inventory
            if (sortType == "All")
            {
                // if we have 34 or less (space at the top and bottom)
                if (playerInv.Count <= 34)
                {
                    for (int i = 0; i < playerInv.Count; i++)   // for all items in our list
                    {
                        // show a button with the item name
                        if (GUI.Button(new Rect(
                            0.5f *  screenSize.x,
                            0.25f * screenSize.y + i * (0.25f * screenSize.y),
                            3 *     screenSize.x,
                            0.25f * screenSize.y), playerInv[i].Name))
                        {
                            // if that button is pressed then that item is the item we have selected
                            selectedItem = playerInv[i];
                        }
                    }
                }
                // more than 34 items 
                else
                {
                    // our movable scroll position
                    scrollPos =
                        // the start of our viewable area
                        GUI.BeginScrollView(
                            // our view window
                            new Rect(0, 0.25f * screenSize.y, 3.75f * screenSize.x, 8.5f * screenSize.y),
                            // our current scroll position within that window
                            scrollPos,
                            // scroll zone (extra space) that we can move within the view window
                            new Rect(0, 0, 0, playerInv.Count * 0.25f * screenSize.y),
                            // can we see the horizontal bar?
                            false,
                            // can we see the vertical bar?
                            true
                        );  // end GUI.BeginScrollView

                    #region Elements inside Scroll View
                    for (int i = 0; i < playerInv.Count; i++)   // for all items in our list
                    {
                        // show a button with the item name
                        if (GUI.Button(new Rect(
                            0.5f * screenSize.x,
                            i * (0.25f * screenSize.y),
                            3 * screenSize.x,
                            0.25f * screenSize.y), playerInv[i].Name))
                        {
                            // if that button is pressed then that item is the item we have selected
                            selectedItem = playerInv[i];
                        }
                    }
                    #endregion
                    // End Scroll View else you get a pushing more clips error
                    GUI.EndScrollView();
                }
            }
            // else we are displaying based off Type
            else
            {
                ItemTypes type = (ItemTypes)System.Enum.Parse(typeof(ItemTypes), sortType);

                // amount of that type
                int a = 0;
                // slot position
                int s = 0;
                // search our list and if we find a match
                for (int i = 0; i < playerInv.Count; i++)
                {
                    if (playerInv[i].Type == type)
                    {
                        a++; // increase the count of that type so we know how many we have
                    }
                }
                // we are less than or equal to 34 items of this type
                if (a <= 34)
                {
                    for (int i = 0; i < playerInv.Count; i++)
                    {
                        if (playerInv[i].Type == type)
                        {
                            if (GUI.Button(new Rect(
                           0.5f * screenSize.x,
                           0.25f * screenSize.y + s * (0.25f * screenSize.y),
                           3 * screenSize.x,
                           0.25f * screenSize.y), playerInv[i].Name))
                            {
                                selectedItem = playerInv[i];
                            }
                            s++;
                        }
                    }
                }
                // more than 34 items of this type
                else
                {
                    // our movable scroll position
                    scrollPos = GUI.BeginScrollView(
                        new Rect(0, 0.25f * screenSize.y, 3.75f * screenSize.x, 8.5f * screenSize.y),
                        scrollPos,
                        new Rect(0, 0, 0, a * 0.25f * screenSize.y),
                        false, true);

                    #region Elements inside Scroll View
                    for (int i = 0; i < playerInv.Count; i++)   // for all items in our list
                    {
                        if (playerInv[i].Type == type)
                        {
                            // show a button with the item name
                            if (GUI.Button(new Rect(
                                0.5f * screenSize.x,
                                s * (0.25f * screenSize.y),
                                3 * screenSize.x,
                                0.25f * screenSize.y), playerInv[i].Name))
                            {
                                // if that button is pressed then that item is the item we have selected
                                selectedItem = playerInv[i];
                            }
                            s++;
                        }
                    }
                    #endregion
                    // End Scroll View else you get a pushing more clips error
                    GUI.EndScrollView();
                }
            }
        }

        void UseItem()
        {
            // store GameManager.scr values in a Vector3 so we don't have to retype GameManager.scr.x etc all the time
            Vector3 screenSize = GameManager.scr;

            // Boxes
            // empty box - 
            // Icon - 
            // Name - 
            GUI.Box(new Rect(4f * screenSize.x, 0.75f * screenSize.y, 3.5f * screenSize.x, 7f * screenSize.y), "");
            GUI.Box(new Rect(4.25f * screenSize.x, 1f * screenSize.y, 3 * screenSize.x, 3f * screenSize.y), selectedItem.Icon);
            GUI.Box(new Rect(4.55f * screenSize.x, 4f * screenSize.y, 2.5f * screenSize.x, 0.5f * screenSize.y), selectedItem.Name);

        }

        private void OnGUI()
        {
            // store GameManager.scr values in a Vector3 so we don't have to retype GameManager.scr.x etc all the time
            Vector3 screenSize = GameManager.scr;

            if (showInv)
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Inventory");
                for (int i = 0; i < enumTypesForItems.Length; i++)
                {
                    if (GUI.Button(new Rect
                        (4f * screenSize.x + i * screenSize.x,
                        screenSize.y * 0.5f,
                        screenSize.x,
                        0.25f * screenSize.y), enumTypesForItems[i]))
                    {
                        sortType = enumTypesForItems[i];
                    }
                }
                Display();
                if (selectedItem != null)
                { UseItem(); }
            }
           

        }
    }

}
