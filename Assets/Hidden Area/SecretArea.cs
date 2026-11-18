using UnityEngine;

public class SecretArea : MonoBehaviour
{
    [SerializeField] private GameObject insideAreaGraphics;

    private void Start()
    {
        if (insideAreaGraphics != null)
        {
            insideAreaGraphics.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Player"))

        {
            insideAreaGraphics.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            insideAreaGraphics.SetActive(true);
        }
    }
}
