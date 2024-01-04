using UnityEngine;

[CreateAssetMenu(fileName = "Character Model", menuName ="Data/Character Model")]
public class CharacterModel : ScriptableObject
{
    [SerializeField]
    private string displayName;

    [SerializeField]
    private float speed = 1f,
        agility = 1f, 
        stamina = 1f;

    [SerializeField]
    private Color mainColor = Color.cyan;

    public string DisplayName => displayName;
    public float Speed => speed;
    public float Agility => agility;
    public float Stamina => stamina;    
    public Color MainColor => mainColor;

    public void RandomizeStats()
    {
        Color.RGBToHSV(mainColor, out float H, out float S, out float V);
        mainColor = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);

        speed = Random.Range(1, 10);
        agility = Random.Range(60, 180);
        stamina = Random.Range(1, 10);

    }
}
