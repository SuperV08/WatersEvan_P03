using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "TextData_", menuName = "TextData/Text Customization")]
public class TextData : ScriptableObject
{
    [Header("Text Customization")]

    [SerializeField] Color _textColor;
    [SerializeField] TMP_FontAsset _font;
}
