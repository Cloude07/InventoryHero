using Cysharp.Threading.Tasks;
using UnityEditor.Overlays;
using UnityEngine;

namespace CloudeDev.Boot.SaveSystema
{
    public class SaveLoaderManager
    {
        private const string SAVE_KEY = "InventoryHeroSave";

        public async UniTask<SaveData> LoadData()
        {
            await UniTask.SwitchToThreadPool(); 
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string json = PlayerPrefs.GetString(SAVE_KEY);
                var data = JsonUtility.FromJson<SaveData>(json);
                await UniTask.SwitchToMainThread();
                return data;
            }
            await UniTask.SwitchToMainThread();
            return new SaveData();
        }

        public async UniTask SaveData(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            await UniTask.CompletedTask;
        }
    }
}