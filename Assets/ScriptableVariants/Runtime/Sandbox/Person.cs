using UnityEngine;

[CreateAssetMenu]
public class Person : ScriptableObject
{
    public string name;
    public Transform prefab;
    public int age;
    public Vector2 size;
    public string[] friends;
    public Transform prefab1;
    public int legs;
    
    [ContextMenu("Add age")]
    void AddAge()
    {
        age += 1;
    }
}