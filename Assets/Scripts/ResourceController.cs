using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public static ResourceController instance;

    [SerializeField] private int woodCount;
    [SerializeField] private int metalCount;
    [SerializeField] private int clothCount;
    [SerializeField] private int otherCount;

    public enum ResourceType { wood, metal, cloth, other }
    public ResourceType resourceType { get; private set; }

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
        textField.text = $"Wood: {woodCount} \nMetal: {metalCount} \nCloth: {clothCount} \nOther: {otherCount}";
    }

    public void GiveResources(int woodAmount, int metalAmount, int clothAmount, int otherAmount)
    {
        woodCount += woodAmount;
        metalCount += metalAmount;
        clothCount += clothAmount;
        otherCount += otherAmount;
    }

    public int GetResource(ResourceType resourceToReturn)
    {
        switch (resourceToReturn)
        {
            case ResourceType.wood:
                return woodCount;
                break;
            case ResourceType.metal:
                return metalCount;
                break;
            case ResourceType.cloth:
                return clothCount;
                break;
            case ResourceType.other:
                return otherCount;
                break;
            default:
                print($"Unable to parse resource type {resourceToReturn}");
                return 0;
                break;
        }
    }
}
