using System.Collections;
using UnityEngine;

public static class AnimatorUtils
{
    /// <summary>
    /// Ждёт, пока Animator доиграет состояние stateName, или выйдет из него.
    /// </summary>
    public static IEnumerator WaitForState(
        Animator animator,
        string stateName,
        int layer = 0,
        float enterTimeout = 0.5f,
        float maxDuration = 5f)
    {
        if (animator == null) yield break;

        // 1. Дожидаемся перехода В состояние (переход ~0.1–0.2s)
        float elapsed = 0f;
        while (elapsed < enterTimeout &&
               !animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. Ждём, пока состояние НЕ закончится (normalizedTime >= 1) или НЕ сменится
        elapsed = 0f;
        while (elapsed < maxDuration)
        {
            var state = animator.GetCurrentAnimatorStateInfo(layer);

            // вышли из состояния — анимация завершилась
            if (!state.IsName(stateName)) yield break;

            // доиграли до конца и не в переходе — завершилась
            if (state.normalizedTime >= 1f && !animator.IsInTransition(layer))
                yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }
        // если дошли сюда — сработал maxDuration (страховка от зависания)
    }
}