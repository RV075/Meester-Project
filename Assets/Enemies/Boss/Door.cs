using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public IEnumerator Close()
    {
        float duration = 2.5f;
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + -transform.up * 5f;

        while (timer < duration)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }


    public IEnumerator Open()
    {
        float duration = 2.5f;
        float timer = 0f;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.up * 5f;

        while (timer < duration)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

}
