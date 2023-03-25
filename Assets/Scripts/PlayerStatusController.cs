using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusController : MonoBehaviour
{
    public static PlayerStatusController instance;

    [SerializeField] private int health = 100;
    [SerializeField] private int sanity = 100;
    [SerializeField] private int hunger = 100;

    private float sanityLossTime = 0f;
    private float hungerLossTime = 0f;

    [SerializeField] Text textField;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        hungerLossTime += Time.deltaTime;
        if (hungerLossTime >= 40f)
        {
            print("Player is hungry");
            hunger -= 2;
            hungerLossTime = 0f;
        }

        sanityLossTime += Time.deltaTime;
        if (sanityLossTime >= 25f)
        {
            print("Player needs a fix");
            sanity -= 5;
            sanityLossTime = 0f;
        }

        textField.text = $"Health: {health} \nSanity: {sanity} \nHunger: {hunger}";
    }
}
