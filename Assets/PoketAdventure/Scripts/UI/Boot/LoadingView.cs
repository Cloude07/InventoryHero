using TMPro;
using UnityEngine;

namespace CloudeDev.UI.Boot
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private TMP_Text _Text;
        public void SetActiveScreen(bool value)
        {
            _loadingScreen.SetActive(value);
            _Text.SetText("Loading...");
        }
    }
}
