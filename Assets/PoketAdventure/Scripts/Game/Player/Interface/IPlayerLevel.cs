using System;

namespace CloudeDev.Game.Player
{
    public interface IPlayerLevel
    {
        event Action<int> OnLevelUp;
        void LevelUp(int level);
    }

}


