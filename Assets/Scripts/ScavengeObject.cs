using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengeObject : InteractObject
{
    [SerializeField] int woodAmount;
    [SerializeField] int metalAmount;
    [SerializeField] int clothAmount;
    [SerializeField] int otherAmount;

    public override void StartInteract()
    {
        ResourceController.instance.GiveResources(woodAmount, metalAmount, clothAmount, otherAmount);
        print($"Scavenged resources from {this.gameObject.name}: wood = {woodAmount}, metal = {metalAmount}, cloth = {clothAmount}, and other = {otherAmount}");
    }

    public override void EndInteract()
    {
        Destroy(this.gameObject);
    }
}
