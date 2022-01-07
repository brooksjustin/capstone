using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockRoom : RoomReset
{
    public Door[] doors;
    private int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = enemies.Length;
        OpenDoors();
    }
    public void CheckEnemies()
    {
        enemyCount =enemyCount-1;
        if (enemyCount <= 0)
        {
            OpenDoors();
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(true);
        }
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (enemyCount > 0)
            {
                CloseDoors();
            }
        }

    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }
    public void CloseDoors()
    {
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].Close();
        }
    }
    public void OpenDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].Open();
        }
    }
}
