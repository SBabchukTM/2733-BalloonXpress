using Runtime.Game;
using TMPro;
using UnityEngine;

public class LeaderboardRecordDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _placeText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _balanceText;

    public void Initialize(LeaderboardRecord record)
    {
        _placeText.text = record.Place.ToString();
        _nameText.text = record.Name.ToString();
        _balanceText.text = record.Balance.ToString();
    }
}
