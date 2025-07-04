using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
