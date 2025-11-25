using Asteroids.Logic.Common.Services.Saving;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Logic.Common.UI.Implementation
{
    public class SaveDataView : MonoBehaviour
    {
        public event Action<SaveData> OnDataChoiced;

        [SerializeField] private TMP_Text _dateTMP;
        [SerializeField] private TMP_Text _bestScoreTMP;
        [SerializeField] private TMP_Text _adsDisabledTMP;
        [SerializeField] private Button _choiceButton;

        private SaveData _data;

        private void Awake()
        {
            _choiceButton.onClick.AddListener(ChoiceButtonClickedHandler);
        }

        private void OnDestroy()
        {
            _choiceButton.onClick.RemoveListener(ChoiceButtonClickedHandler);
        }

        public void Show(SaveData saveData)
        {
            _data = saveData;
            _dateTMP.SetText(TimeSpan.FromTicks(saveData.Timestamp).ToString());
            _bestScoreTMP.SetText(saveData.BestScore.ToString());
            _adsDisabledTMP.SetText(saveData.AdsDisabled ? "Disabled" : "Enabled");
        }

        private void ChoiceButtonClickedHandler()
        {
            OnDataChoiced?.Invoke(_data);
        }
    }
}