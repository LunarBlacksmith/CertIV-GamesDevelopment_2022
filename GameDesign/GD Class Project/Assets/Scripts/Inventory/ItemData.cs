using UnityEngine;

public static class ItemData
{
    public static Item CreateItem(int itemId_p)
    {
        string _name        = "";
        string _description = "";
        string _icon        = "";
        string _mesh        = "";
        int _value  = 0;
        int _amount = 0;
        int _damage = 0;
        int _armour = 0;
        int _heal   = 0;
        ItemTypes _type = ItemTypes.Apparel;

        switch (itemId_p)
        {
            #region Food 0-99
            case 0:
                {
                    _name = "Apple";
                    _description = "This is an Apple";
                    _icon = "Food/Apple";
                    _mesh = "Food/Apple";
                    _value = 1;
                    _amount = 1;
                    _heal = 10;
                    _type = ItemTypes.Food;
                
                    break;
                }
            case 1:
                _name = "Banana";
                _description = "This is a banana";
                _icon = "Food/Banana";
                _mesh = "Food/banana";
                _value = 3;
                _amount = 5;
                _heal = 15;
                _type = ItemTypes.Food;
                break ;
            #endregion
            #region Weapons 100-199
            case 100:
                _name = "Axe";
                _description = "This is an Axe";
                _icon = "Weapon/Axe";
                _mesh = "Weapon/Axe";
                _value = 3;
                _amount = 5;
                _heal = 15;
                _type = ItemTypes.Weapon;
                break;
            #endregion
            #region Apparel 200-299
            #endregion
            #region Crafting 300-399
            #endregion
            #region Ingredients 400-499
            #endregion
            #region Potions 500-599
            #endregion
            #region Scrolls 600-699
            #endregion
            #region Quest 700-799
            #endregion
            #region Misc 800-999
            #endregion
            default:
                {
                    itemId_p = 0;
                    _name = "Apple";
                    _description = "This is an Apple";
                    _icon = "Food/Apple";
                    _mesh = "Food/Apple";
                    _value = 1;
                    _amount = 1;
                    _heal = 10;
                    _type = ItemTypes.Food;
                    break;
                }
                
        }

        Item temp = new Item()
        {
            Id      = itemId_p,
            Name    = _name,
            Description = _description,
            Value   = _value,
            Amount  = _amount,
            Type    = _type,
            Icon    = Resources.Load("Icon/" + _icon) as Texture2D,
            Prefab  = Resources.Load("Prefab/" + _mesh) as GameObject,
            Damage  = _damage,
            Armour  = _armour,
            Heal    = _heal,
        };

        return temp;
    }
}
