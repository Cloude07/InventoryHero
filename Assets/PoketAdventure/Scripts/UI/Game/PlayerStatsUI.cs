using CloudeDev.Game.Enemy.Pool;
using CloudeDev.Game.Player;
using CloudeDev.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CloudeDev.Game.Ui
{
    public class PlayerStatsUI : MonoBehaviour
    {
        private IPlayerStatsReadonly _playerStats;
        private IPlayerView _playerView;

        [SerializeField] private StatsDisplayUI _statsDisplayUI;
        [SerializeField] private Button _nextButton;
        [SerializeField] private EnemyPool _enemyPool;

        [Inject]
        private void Construct(IPlayerStatsReadonly playerStatsReadonly, IPlayerView playerView)
        {
            _playerStats = playerStatsReadonly;
            _playerView = playerView;

            _playerStats.OnStatsChanged += OnStatsChanged;
       
        }

        private void Start()
        {
            _nextButton.onClick.AddListener(OnClickNext);
        }

        private void OnNextWaveChange()
        {
            _statsDisplayUI.NextWaveIsActive(true);
        }

        private void OnStatsChanged(StatsPlayer stats, int value)
        {
            switch (stats)
            {
                case StatsPlayer.Health:
                    _statsDisplayUI.ChangeTextHeath(value.ToString());
                    break;
                case StatsPlayer.Dexterity:
                    _statsDisplayUI.ChangeTextDexterity(value.ToString());
                    break;
                case StatsPlayer.Strength:
                    _statsDisplayUI.ChangeTextStrength(value.ToString());
                    break;
                case StatsPlayer.Armor:
                    _statsDisplayUI.ChangeTextArmor(value.ToString());
                    break;
            }
        }

        private void OnDisable()
        {
            _playerStats.OnStatsChanged -= OnStatsChanged;
            _nextButton.onClick.RemoveListener(OnClickNext);

        }

        private void ShowLevelUpEffect(int value)
        {
           
        }

        private void OnClickNext()
        {
            _statsDisplayUI.NextWaveIsActive(false);

            _playerView.IsRunning();
            _enemyPool.SpawnNextEnemy();
        }



        public void OnClickVisible() => 
            _nextButton.gameObject.SetActive(true);

        private void UpdateCoinUI(int value) =>
          _statsDisplayUI.ChangeTextCoinCurrent(value.ToString());
        


    }
}
