using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button _start;
    [SerializeField] Button _continue;
    [SerializeField] Button _exit;
    [Space]
    [SerializeField] CurrentLevelPosition _currentLevelPosition;

    private void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerSaveData.dat"))
        {
            _currentLevelPosition.numberOfCurrentPointPos = 0;
            _continue.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerSaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/PlayerSaveData.dat");
        }
        for (int i = 0; i < 16; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/Doll" + i.ToString() + ".dat"))
            {
                File.Delete(Application.persistentDataPath + "/Doll" + i.ToString() + ".dat");
            }
        }
        if (File.Exists(Application.persistentDataPath + "/Biomass0.dat"))
        {
            File.Delete(Application.persistentDataPath + "/Biomass0.dat");
        }
        SceneManager.LoadScene("Test Level");
    }
    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/PlayerSaveData.dat", FileMode.Open);
        PlayerSaveData data = (PlayerSaveData)bf.Deserialize(file);
        file.Close();

        _currentLevelPosition.numberOfCurrentPointPos = data._savedCurrentLevelPosition;
        SceneManager.LoadScene(data._savedCurrentSceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
