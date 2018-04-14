using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadManager
{
    private static string LoadIn = Application.persistentDataPath + "/save.data";

    public static void SaveData(DataContainer_Character savedata)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(LoadIn, FileMode.Create);

        Data file = new Data(savedata);

        bf.Serialize(stream, file);
        stream.Close();
    }

    public static Data LoadData()
    {
        if (File.Exists(LoadIn))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(LoadIn, FileMode.Open);

            Data file = bf.Deserialize(stream) as Data;

            stream.Close();
            return file;
        }
        else
        {
            return new Data();
        }
    }

}

[Serializable]
public class Data
{
    public int[] scores;
    public string[] nicks;

    public bool contains;
    public int levelsComplete;

    public Data()
    {
        contains = false;
        levelsComplete = 0;
        scores = new int[0];
        nicks = new string[0];
    }

    public Data(DataContainer_Character container)
    {
        contains = true;
        levelsComplete = container.plays;

        scores = new int[container.scores.Count];
        nicks = new string[container.nicknames.Count];

        for (int i = 0; i < container.scores.Count; i++)
        {
            scores[i] = container.scores[i];
        }
        for (int j = 0; j < container.nicknames.Count; j++)
        {
            nicks[j] = container.nicknames[j];
        }
    }
}