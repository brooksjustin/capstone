using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransistion : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosistion;
    public VectorValue playerStorage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            playerStorage.initialValue = playerPosistion;
            SceneManager.LoadScene(sceneToLoad);
        }
    }



}
