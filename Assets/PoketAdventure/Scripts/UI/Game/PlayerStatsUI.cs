using CloudeDev.Game.Combat;
using CloudeDev.Game.Enemy.Pool;
using CloudeDev.Game.Player;
using CloudeDev.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CloudeDev.Game.Ui
{
    public class PlayerStatsUI : MonoBehaviour
    {
        private IPlayerStatsReadonly _playerStats;
        private IPlayerView _playerView;
        private bool _subscribed;

        [SerializeField] private StatsDisplayUI _statsDisplayUI;
        [SerializeField] private Button _nextButton;
        [SerializeField] private EnemyPool _enemyPool;
        [SerializeField] private TMP_Text _combatLog;
        [SerializeField] private TurnBasedCombatController _combat;

        [Inject]
        private void Construct(IPlayerStatsReadonly playerStatsReadonly, IPlayerView playerView)
        {
            _playerStats = playerStatsReadonly;
            _playerView = playerView;
            Subscribe();
        }

        private void OnEnable()
        {
            Subscribe();
            if (_combat != null)
                _combat.OnCombatLog += msg => _combatLog.SetText(msg);
        }
        private void OnDisable() => Unsubscribe();

        private void Start()
        {
            RefreshAllStats();
        }

        private void Subscribe()
        {
            if (_subscribed || _playerStats == null) return;
            _playerStats.OnStatsChanged += OnStatsChanged;
            if (_nextButton != null)
                _nextButton.onClick.AddListener(OnClickNext);
            _subscribed = true;
            RefreshAllStats();
        }

        private void Unsubscribe()
        {
            if (!_subscribed) return;
            _playerStats.OnStatsChanged -= OnStatsChanged;
            if (_nextButton != null)
                _nextButton.onClick.RemoveListener(OnClickNext);
            _subscribed = false;
        }

        private void RefreshAllStats()
        {
            if (_playerStats == null) return;
            _statsDisplayUI.ChangeTextHeath(_playerStats.Health.ToString());
            _statsDisplayUI.ChangeTextArmor(_playerStats.Armor.ToString());
            _statsDisplayUI.ChangeTextStrength(_playerStats.Strength.ToString());
            _statsDisplayUI.ChangeTextDexterity(_playerStats.Dexterity.ToString());
            _statsDisplayUI.ChangeTextCoinCurrent(_playerStats.Coin.ToString());
            _statsDisplayUI.ChangeSliderHealth(_playerStats.Health, _playerStats.MaxHealth);
        
        }

        private void OnStatsChanged(StatsPlayer stats, int newValue)
        {
            switch (stats)
            {
                case StatsPlayer.Health:
                    _statsDisplayUI.ChangeSliderHealth(newValue, _playerStats.MaxHealth); 
                    break;
                case StatsPlayer.MaxHealth:
                    _statsDisplayUI.ChangeSliderHealth(_playerStats.Health, newValue);
                    _statsDisplayUI.ChangeTextHeath(newValue.ToString());
                    break;
                case StatsPlayer.Dexterity:
                    _statsDisplayUI.ChangeTextDexterity(newValue.ToString());
                    break;
                case StatsPlayer.Strength:
                    _statsDisplayUI.ChangeTextStrength(newValue.ToString());
                    break;
                case StatsPlayer.Armor:
                    _statsDisplayUI.ChangeTextArmor(newValue.ToString());
                    break;
                case StatsPlayer.Coin:
                    _statsDisplayUI.ChangeTextCoinCurrent(newValue.ToString());
                    break;
            }
        }

        private void OnClickNext()
        {
            _statsDisplayUI.NextWaveIsActive(false);
            _playerView.IsRunning();
            _enemyPool.SpawnNextEnemy();
        }

        public void OnClickVisible() => _nextButton.gameObject.SetActive(true);
    }
}
