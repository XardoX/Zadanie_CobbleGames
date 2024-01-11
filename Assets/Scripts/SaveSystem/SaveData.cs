using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{ 
    [System.Serializable]
    public class SaveData
    {
        public List<UnitData> units = new();

        public SaveData()
        {
            units = new();
        }

    }
    [System.Serializable]
    public class UnitData
    {
        private CharacterModel characterModel;

        public Vector3 position;

        public Quaternion rotation;

        [SerializeField] private string serializedModel;

        public CharacterModel CharacterModel
        {
            get
            {
                var model = ScriptableObject.CreateInstance<CharacterModel>();
                JsonUtility.FromJsonOverwrite(serializedModel, model);
                return model;
            }
        }

        public UnitData(CharacterModel characterModel, Vector3 position, Quaternion rotation)
        {
            this.characterModel = characterModel;
            this.position = position;
            this.rotation = rotation;

            this.serializedModel = JsonUtility.ToJson(characterModel);
        }


    }
}
