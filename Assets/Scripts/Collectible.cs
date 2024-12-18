using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int _collectibleValue;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private AudioClip _takeSFX;

    private void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null && other.GetComponent<Player>() != null)
        {
            AudioManager.Instance.PlaySFX(_takeSFX);
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            if(particleSystem != null)
            {
                particleSystem.Play();
                particleSystem.transform.parent = null;
            }
            gm.OnItemCollected(_collectibleValue);
            Destroy(gameObject);
        }
    }
}
