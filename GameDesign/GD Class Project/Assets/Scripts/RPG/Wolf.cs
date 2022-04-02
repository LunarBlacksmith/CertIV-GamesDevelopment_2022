using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : EnemyMovement
{
    public Material[] wolfSkin = new Material[4];
    public new Renderer renderer;

    public override void Start()
    {
        base.Start();
        Debug.Log("AHHHH");
        //difficulty 1-4
        difficulty = Random.Range(1, 5);
        //maximum Health = our Constitution * 5 * difficulty
        attributes[0].maxValue = baseStats[2].value * 5 * difficulty;
        //current Health is equal to maximum Health
        attributes[0].curValue = attributes[0].maxValue;
        //base damage is equal to Strength * 2
        baseDamage = baseStats[0].value * 2;
        walkSpeed = baseStats[1].value * 2;
        runSpeed = baseStats[1].value * 3;
        renderer = GetComponentInChildren<Renderer>();
        renderer.material = wolfSkin[difficulty-1];
    }

    public void BiteAttack()
    {
        //1 - 20 because the numbers are ints. Floats are inclusive of max
        int critChance = Random.Range(1,21);
        float critDamage = 0f;
        if (critChance >= critAmount)
        {
            critDamage = Random.Range(baseDamage/2, baseDamage * difficulty);
        }

        Debug.Log(baseDamage * difficulty * critDamage);
        /* player.GetComponent<PlayerHandler>().DamagePlayer(baseDamage * difficulty * critDamage); */
    }
}
