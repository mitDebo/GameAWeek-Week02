using UnityEngine;
using System.Collections;

public class FloatingTextMotor : MonoBehaviour {
    public float RaisingSpeed;
    public float HoldTime;
    public float FadeOutTime;

    Transform mTransform;
    UILabel label;

    void Awake()
    {
        mTransform = transform;
        label = GetComponent<UILabel>();
        StartCoroutine("fadeOut", HoldTime * 1000);
    }

    void FixedUpdate()
    {
        mTransform.position = new Vector3(mTransform.position.x, mTransform.position.y + (RaisingSpeed * Time.deltaTime), mTransform.position.z);
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(HoldTime);

        float timer = FadeOutTime;
        while (timer >= 0)
        {
            label.alpha = timer / FadeOutTime;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
