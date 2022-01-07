using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceManager : MonoBehaviour
{
    public Slider resourceSlider;
    public Inventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        resourceSlider.maxValue = playerInventory.maxResource;
        resourceSlider.value = playerInventory.maxResource;
        playerInventory.currentResource = playerInventory.maxResource;
    }

    public void AddResource()
    {
        resourceSlider.value = playerInventory.currentResource;
        if (resourceSlider.value > resourceSlider.maxValue)
        {
            resourceSlider.value = resourceSlider.maxValue;
            playerInventory.currentResource = playerInventory.maxResource;
        }
    }
    public void DecreaseResource()
    {
        resourceSlider.value = playerInventory.currentResource;
        if (resourceSlider.value < 0)
        {
            resourceSlider.value = 0;
            playerInventory.currentResource = 0;
        }
    }
}
