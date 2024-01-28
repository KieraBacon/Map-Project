using UnityEngine;

[CreateAssetMenu(fileName = "New Zone Info", menuName = "Map Project/Zone Info")]
public class ZoneData : ScriptableObject, IScreenData
{
    [SerializeField, TextArea(1, 100)] private string _description;

    public string HeaderText => name;
    public string BodyText => _description;
}