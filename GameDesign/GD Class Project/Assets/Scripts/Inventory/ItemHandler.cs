using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemHandler : MonoBehaviour
{
    public int itemID;
    public int amount;
    public ItemTypes itemType;

    public void OnCollection()
    {
        if (itemType == ItemTypes.Money)
        { Inventory.Player.Inventory.money += amount; }
        else if (itemType == ItemTypes.Weapon || itemType == ItemTypes.Apparel || itemType == ItemTypes.Quest)
        { Inventory.Player.Inventory.playerInv.Add(ItemData.CreateItem(itemID)); }
        else
        {
            bool found = false;
            int addIndex = 0;
            for (int i = 0; i < Inventory.Player.Inventory.playerInv.Count; i++)
            {
                if (itemID == Inventory.Player.Inventory.playerInv[i].Id)
                {
                    found = true;
                    addIndex = i;
                    break;
                }
            }
            if (found)
            { Inventory.Player.Inventory.playerInv[addIndex].Amount += amount; }
            else
            {
                Inventory.Player.Inventory.playerInv.Add(ItemData.CreateItem(itemID));
                Inventory.Player.Inventory.playerInv.Last<Item>().Amount = amount;
            }
        }
        Destroy(gameObject);
    }
}
