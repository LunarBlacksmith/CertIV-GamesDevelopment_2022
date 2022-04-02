using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Canvas Element Image

public class GradientHealth : Stats
{
    public Gradient gradient;
    public Canvas enemyHealthDisplay;
    Transform can;

    public override void Start()
    {
        base.Start();
        Debug.Log("AHH 3");

        can = Camera.main.transform;
    }

    public virtual void Update()
    {
        SetHealth();
        enemyHealthDisplay.transform.LookAt(enemyHealthDisplay.transform.position + can.forward);
    }

    public void SetHealth()
    {
        //creates a % of health left between cur and max to then show how full our health bar is.
        attributes[0].display.fillAmount = Mathf.Clamp01(attributes[0].curValue / attributes[0].maxValue);
        //taking the amount on our health bar change the colour according to a gradient
        attributes[0].display.color = gradient.Evaluate(attributes[0].display.fillAmount);
    }
}
