using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public List<ScriptableObject> objects = new List<ScriptableObject>();
    private void OnEnable()
    {
        LoadScriptables();
    }
    private void OnDisable()
    {
    }
    public void SaveScriptables()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.dat", i));
            BinaryFormatter binary = new BinaryFormatter();
            var json = JsonUtility.ToJson(objects[i]);
            binary.Serialize(file, json);
            file.Close();
        }
    }
    public void LoadScriptables()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);
                BinaryFormatter binary = new BinaryFormatter();
                JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), objects[i]);
                file.Close();
            }
        }
    }
    public void ResetScriptables()
    {
        for (int i = 0; i < objects.Count; i++)
        {

            switch (objects[i].GetType().FullName)
            {
                case "FloatValue":
                    FloatValue ftmp = (FloatValue)objects[i];
                    ftmp.RunTimeValue = ftmp.initialValue;
                    break;

                case "BoolValue":
                    BoolValue btmp = (BoolValue)objects[i];
                    btmp.RunTimeValue = btmp.initialValue;
                    break;

                case "Inventory":
                    Inventory itmp = (Inventory)objects[i];
                    itmp.coins = 0;
                    itmp.maxResource = 10;
                    itmp.items.Clear();
                    itmp.numberOfKeys = 0;
                    break;
                default:
                    break;
            }

            if (File.Exists(Application.persistentDataPath +
                string.Format($"/{i}.dat")))
            {
                File.Delete(Application.persistentDataPath +
                string.Format($"/{i}.dat"));
            }
        }
    }
}
