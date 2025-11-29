using UnityEngine;

public class PlayerSoundColliding : MonoBehaviour
{
    [SerializeField] AudioClip playerCollide;
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Wall")) || (collision.gameObject.CompareTag("Rock")) || (collision.gameObject.CompareTag("Enemy")))
        {
            AudioSource.PlayClipAtPoint(playerCollide, transform.position);
        }
    }
}
