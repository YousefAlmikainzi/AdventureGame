using System.Collections;
using UnityEngine;

public class DissolveApply : MonoBehaviour
{
    [SerializeField] float startDelay = 5f;
    [SerializeField] float duration = 1f;
    [SerializeField] string propertyName = "_DissolveAmount";

    Renderer rend;
    Material mat;
    int propID;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propID = Shader.PropertyToID(propertyName);
        mat = rend.sharedMaterial;
        StartCoroutine(DelayedDissolve());
    }

    IEnumerator DelayedDissolve()
    {
        yield return new WaitForSeconds(startDelay);
        yield return DissolveRoutine(1f, 0f, duration);
    }

    public IEnumerator DissolveRoutine(float from, float to, float dur)
    {
        float t = 0f;
        mat.SetFloat(propID, from);

        while (t < dur)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / dur);
            mat.SetFloat(propID, val);
            yield return null;
        }

        mat.SetFloat(propID, to);
    }
}
