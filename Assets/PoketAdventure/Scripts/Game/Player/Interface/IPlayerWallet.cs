using System;

namespace CloudeDev.Game.Player
{
    public interface IPlayerWallet
    {
        event Action<int> _OnCoinChanged;
        void AddCoin(int value);
        bool RemoveCoin(int value);
    }
}


