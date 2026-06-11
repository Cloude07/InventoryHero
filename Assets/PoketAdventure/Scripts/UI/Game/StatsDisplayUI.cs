using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CloudeDev.UI
{
    [Serializable]
    public class StatsDisplayUI
    {
        [Header("Stats")]
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _armorText;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _dexterityText;

        [Header("Health")]
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private Slider _healthSlider;

        [Header("Interactive")]
        [SerializeField] private Button _nextButton;
   
        public void ChangeTextHeath(string value) =>
            _healthText.SetText($"Heal: {value}");

        public void ChangeTextArmor(string value) =>
            _armorText.SetText($"Arm: {value}");

        public void ChangeTextDexterity(string value) =>
            _dexterityText.SetText($"Dex: {value}");

        public void ChangeTextStrength(string value) =>
            _damageText.SetText($"Dmg: {value}");

        public void ChangeTextCoinCurrent(string value) =>
            _coinText.SetText($"Coin: {value}");

        public void ChangeSliderHealth(int value) =>
            _healthSlider.value = value;

        public void NextWaveIsActive(bool isActive) =>
            _nextButton.gameObject.SetActive(isActive);
    }
}
