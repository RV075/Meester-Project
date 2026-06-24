using System.Collections;
using UnityEngine;
public class FocusCamera : MonoBehaviour
{
    public static FocusCamera instance;
    [SerializeField] private GameObject boss;

    private Camera cam;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }
    public IEnumerator Focus(float duration, Transform target, float speed)
    {
        float timer = 0;
        while (timer < duration)
        {
            Vector3 lerp = Vector3.Slerp(
                cam.transform.position,
                target.position,
                speed * Time.deltaTime);

            cam.transform.position = lerp;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator UnFocus(float duration, Vector3 origin, float speed)
    {
        float timer = 0;
        while (timer < duration)
        {
            Vector3 lerp = Vector3.Lerp(
                cam.transform.position,
                origin,
                speed * Time.deltaTime);

            cam.transform.position = lerp;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
