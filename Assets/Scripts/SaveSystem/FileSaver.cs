using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveSystem
{
    public class FileSaver
    {

        private string savePath;

        public FileSaver(string path, string fileName)
        {
            savePath = Path.Combine(path, fileName);
        }

        public SaveData Load()
        {
            SaveData data = null;
            
            if(IsGameSaved())
            {
                var dataToLoad = "";
                using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
                {
                    using (StreamReader reader =  new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                data = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            return data;
        }

        public async Task<SaveData> LoadAsync()
        {
            var result = await Task.Run(() =>
            {
                return Load();
            });

            return result;
        }

        public void Save(SaveData data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            var dataToSave = JsonUtility.ToJson(data, true);

            using FileStream fileStream = new(savePath, FileMode.Create);
            using StreamWriter writer = new(fileStream);
            writer.Write(dataToSave);
        }

        public async void SaveAsync(SaveData data)
        {
            var result = await Task.Run(() =>
            {
                Save(data);
                return true;
            });
        }

        public bool IsGameSaved()
        {
            return File.Exists(savePath);
        }

    }
}
