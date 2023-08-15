using UnityEngine;

public class MonoPerson : MonoBehaviour
{
    [Header("pp")]
    public string name2;
    public int age;
    public Vector2 size;
    [NonReorderable]
    public string[] friends;

    [ContextMenu("Hide Transform")]
    void HidTrans()
    {
        transform.hideFlags = HideFlags.HideInInspector;
    }
}