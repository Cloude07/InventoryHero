using UnityEngine;

namespace CloudeDev.Game.Player
{
    public interface IPlayerView
    {
        Transform PlayerTransform { get; }
        Transform DamagePoint { get; }
        float DetectionDistance { get; }
        int Health { get; }
        void PlayAnimation(string trigger);
        bool isRunn {  get; }
        void IsRunning();
        void IsFight();
        void Win();
        Animator Animator { get; }

    }
}