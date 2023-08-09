using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public AnimationCurve curve;

    IEnumerator Shaking() {
        Vector3 startPosition = transform.position;

        float elapsed = 0;
        float duration = 0.2f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float strength = curve.Evaluate(elapsed / duration);
            transform.position += Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }

    void ShakeUtil(int x, int y) {
        StartCoroutine(Shaking());
    }

    private void OnEnable()
    {
        GameEvents.OnBomBlastInvoke += ShakeUtil;
    }

    private void OnDisable()
    {
        GameEvents.OnBomBlastInvoke -= ShakeUtil;
    }
}
