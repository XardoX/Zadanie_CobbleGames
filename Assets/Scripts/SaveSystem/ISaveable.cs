namespace SaveSystem
{
    public interface ISaveable
    {
        void LoadData(SaveData data);

        void SaveData(ref SaveData data);
    }
}
