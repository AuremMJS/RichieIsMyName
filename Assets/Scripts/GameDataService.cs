using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Playables;

public class GameDataService
{
    private static GameDataService instance;
    public static GameDataService Instance
    {
        get
        {
            if (instance == null)
                instance = new GameDataService();
            return instance;
        }
    }

    public GameData GameData { get; set; }

    public void SaveGameData()
    {
        string fileName = Application.persistentDataPath + "/GameData.xml";
        FileStream createStream = File.Create(fileName);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
        xmlSerializer.Serialize(createStream, GameData);
        createStream.Close();
    }

    public void LoadGameData()
    {
        string fileName = Application.persistentDataPath + "/GameData.xml";
        FileStream readStream = File.OpenRead(fileName);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameData));
        GameData = (GameData) xmlSerializer.Deserialize(readStream);
        readStream.Close();
    }

    public bool IsSavedGameData()
    {
        string fileName = Application.persistentDataPath + "/GameData.xml";
        return File.Exists(fileName);
    }

    public void DeleteSavedGameData()
    {
        string fileName = Application.persistentDataPath + "/GameData.xml";
        if(File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
