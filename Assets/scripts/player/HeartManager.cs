using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }
    public void InitHearts()
    {
        for(int i=0;i<heartContainers.RunTimeValue;i++)
        {
            if (i < hearts.Length)
            {
                hearts[i].gameObject.SetActive(true);
                hearts[i].sprite = fullHeart;
            }
        }
    }
    public void UpdateHearts()
    {
        InitHearts();
        float tempHealth;
        tempHealth = playerCurrentHealth.RunTimeValue / 2;
        for (int i = 0; i < heartContainers.RunTimeValue; i++)
        {
            if (i <= tempHealth-1)
            {
                //full heart
                hearts[i].sprite = fullHeart;
            }else if (i >= tempHealth)
            {
                //empty heart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //half full heart
                hearts[i].sprite = halfHeart;
            }
        }
    }
}
