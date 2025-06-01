using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI
{
    public class ClueFieldUI : MonoBehaviour
    {
        [SerializeField, Required] private TextMeshProUGUI _fieldNameText;
        [SerializeField, Required] private TextMeshProUGUI _fieldCountText;
        [SerializeField, Required] private string[] _moduleTypes;

        public void Initialize(BombClueType clueType, KeyValuePair<int, int> clue)
        {
            _fieldNameText.text = GetFieldName(clueType, clue.Key);
            _fieldCountText.text = GetFieldCount(clue);
            
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector3 localPos = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(localPos.x, localPos.y, 0f);
            
            rectTransform.localEulerAngles = Vector3.zero;
        }
        
        private string GetFieldName(BombClueType clueType, int index)
        {
            switch (clueType)
            {
                case BombClueType.Number:
                    return (index + 1).ToString();
                case BombClueType.Color: // TODO: Change this to showing color instead of names
                    switch (index)
                    {
                        case 0:
                            return "Red";
                        case 1:
                            return "Green";
                        case 2:
                            return "Blue";
                        case 3:
                            return "Yellow";
                        case 4:
                            return "White";
                    }
                    break;
                case BombClueType.Type:
                    return _moduleTypes[index];
                case BombClueType.Letter:
                    return ((char)('A' + index)).ToString();
            }

            return "NOT FOUND";
        }
        
        private string GetFieldCount(KeyValuePair<int, int> clue)
        {
            return clue.Value.ToString();
        }
    }
}