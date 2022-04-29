using UnityEngine;

public class Item
{
    #region Private Variables
    private int     _id;
    private string  _name;
    private string  _description;
    private int     _value;
    private ItemTypes   _type;
    private Texture2D   _icon;
    private GameObject  _prefab;

    // if it's stackable
    private int _amount;
    // not base
    private int _damage;
    private int _armour;
    private int _heal;


    #endregion
    #region Public Variables
    public int Id 
    { get {return _id;} set {_id = value;} }

    public string Name          
    { get{ return _name; } set{_name = value;} }

    public string Description   
    { get{ return _description; } set{_description = value;} }

    public int Value            
    { get{ return _value; } set{_value = value;} }

    public int Buy 
    {
        get { return Mathf.CeilToInt(_value * 1.25f); }
    }
    public int Sell 
    {
        get { return Mathf.FloorToInt(_value * 1.25f); }
    }

    public ItemTypes Type       
    { get{ return _type; } set{_type = value;} }

    public Texture2D Icon       
    { get{ return _icon; } set{_icon = value;} }

    public GameObject Prefab    
    { get{ return _prefab; } set{_prefab = value;} }

    public int Amount
    { get{ return _amount; } set{_amount = value;} }

    public int Damage           
    { get{ return _damage; } set{_damage = value;} }

    public int Armour           
    { get{ return _armour; } set{_armour = value;} }

    public int Heal             
    { get { return _heal; } set{_heal = value; } }

    #endregion

    
}
public enum ItemTypes
    {
        Food,
        Weapon,
        Apparel,
        Crafting,
        Ingredient,
        Potion,
        Scroll,
        Quest,
        Money
    }