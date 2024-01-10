using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{

    private string _playerDataPath;
    private string _ressourcesDataPath;
    private string _buildingPath;

    [SerializeField] public PlayerManager Player;
    [SerializeField] public RessourcesGenerator Ressources;
    [SerializeField] public Building Building;

    public bool IsFirstLoad = false;

    void OnEnable()
    {

        _playerDataPath = Application.persistentDataPath + "/data.json";
        _ressourcesDataPath = Application.persistentDataPath + "/ressources_data.json";
        _buildingPath = Application.persistentDataPath + "/build_data.json";

        if (File.Exists(_playerDataPath))
        {
            LoadGameData();
        } else
        {
            IsFirstLoad = true;
        }
    }

    void OnApplicationQuit()
    {

        SaveGameData();

    }

    public void SaveGameData()
    {

        string playerData = JsonUtility.ToJson(new PlayerData(Player));
        File.WriteAllText(_playerDataPath, playerData);
        print("Saved at " + _playerDataPath);

        string ressourcesData = JsonUtility.ToJson(Ressources.RessourceSave);
        File.WriteAllText(_ressourcesDataPath, ressourcesData);
        print("Saved at " + _ressourcesDataPath);

        string buildingData = JsonUtility.ToJson(Building.BuildSave);
        File.WriteAllText(_buildingPath, buildingData);
        print("Saved at " + _buildingPath);

    }

    public void LoadGameData()
    {

        string playerJsonString= File.ReadAllText(_playerDataPath);
        PlayerData deserializedPlayerData = JsonUtility.FromJson<PlayerData>(playerJsonString);
        Player.SetValues(deserializedPlayerData);
        print("Loaded player !");

        if (File.Exists(_ressourcesDataPath))
        {
            string ressourcesJsonString = File.ReadAllText(_ressourcesDataPath);
            Ressources.RessourceSave = JsonUtility.FromJson<RessourceSave>(ressourcesJsonString);
            print("Loaded ressources !");
        }
        if (File.Exists(_buildingPath))
        {
            string buildSaveJsonString = File.ReadAllText(_buildingPath);
            Building.BuildSave = JsonUtility.FromJson<BuildSave>(buildSaveJsonString);
            print("Loaded builds !");
        }
    }

}
