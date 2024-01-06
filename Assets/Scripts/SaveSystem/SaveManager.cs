using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        private string fileName = "SaveData";

        private SaveData saveData;

        private List<ISaveable> objectsToSave;

        private FileSaver fileSaver;

        public static SaveManager Instance { get; private set; }
        public void Init()
        {
            objectsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>().ToList();
        }

        public void NewSave()
        {
            saveData = new SaveData();
        }

        public async void LoadData()
        {
            var result = await Task.Run(() =>
            {
                saveData = fileSaver.LoadAsync().Result;
                return saveData;
            });
            foreach (var item in objectsToSave)
            {
                item.LoadData(result);
            }
        }

        public void SaveData()
        {
            if(saveData == null) NewSave();
            if (objectsToSave == null) return;

            foreach (var item in objectsToSave)
            {
                item?.SaveData(ref saveData);
            }
            fileSaver.SaveAsync(saveData);
        }

        public bool IsGameSaved()
        {
            return fileSaver.IsGameSaved();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
            fileSaver = new FileSaver(Application.persistentDataPath, fileName);
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}