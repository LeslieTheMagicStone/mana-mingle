using TMPro;
using UnityEngine;

public class ChatCell : MonoBehaviour
{
    public void Init(string playerName, string content)
    {
        TMP_Text nameText = transform.Find("Name").GetComponent<TMP_Text>();
        nameText.text = playerName;
        TMP_Text contentText = transform.Find("Content").GetComponent<TMP_Text>();
        contentText.text = content;
    }
}
