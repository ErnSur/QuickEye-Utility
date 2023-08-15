using System.Text;
using UnityEngine;
using UnityEngine.UI;

class RuntimeTest : MonoBehaviour
{
    public Text label;

    private void Start()
    {
        var sb = new StringBuilder();
        var eve = Resources.Load<Person>("Eve");
        Debug.Log($"eve {eve.age}");
        foreach (var person in Resources.FindObjectsOfTypeAll<Person>())
        {
            sb.AppendLine($"{person.name}, {person.age}");
        }

        Debug.Log($"{sb}");
        label.text = $"People:\n{sb}";
    }
}