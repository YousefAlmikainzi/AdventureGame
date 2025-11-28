using System.Collections;
using UnityEngine;

public class SwingAttempt : MonoBehaviour
{
    [SerializeField] float AngleDepthX = 60;
    [SerializeField] float AngleDepthY = 60;
    [SerializeField] float AngleDepthZ = 60;

    [SerializeField] float swingDuring = 0.1f;
    [SerializeField] float swingReturn = 0.1f;

    public Quaternion swordRot;
    private bool isSwinging = false;
    void Start()
    {
        swordRot = transform.localRotation;
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(swing());
        }
    }
    IEnumerator swing()
    {
        isSwinging = true;

        float timer = 0.0f;
        Quaternion startingRotation = transform.localRotation;
        Quaternion endRotation = swordRot * Quaternion.Euler(AngleDepthX, AngleDepthY, AngleDepthZ);

        while(timer < swingDuring)
        {
            timer += Time.deltaTime;
            float t = timer / swingDuring;
            transform.localRotation = Quaternion.Lerp(startingRotation, endRotation, t);
            //may change with something else instead of yield return null
            yield return null;
        }

        timer = 0.0f;
        while (timer < swingReturn)
        {
            timer += Time.deltaTime;
            float t = timer / swingReturn;
            transform.localRotation = Quaternion.Lerp(endRotation, swordRot, t);
            yield return null;
        }
        transform.localRotation = swordRot;
        isSwinging = false;
    }
}
