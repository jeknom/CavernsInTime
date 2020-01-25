using UnityEngine;

public interface IObstacle
{
    void ReduceHealth(int amount);
}

public class Obstacle : MonoBehaviour, IObstacle
{
    [SerializeField] int health = 3;
    [SerializeField] float interactDistance = 1f;
    [SerializeField] Sprite[] stateSprites;
    SpriteRenderer spriteRendrer;

    void Start()
    {
        if (this.stateSprites.Length != health)
        {
            Debug.LogError("Obstacle health and amount of state sprites must be equal.");
        }

        this.spriteRendrer = this.GetComponent<SpriteRenderer>();
    }

    public void ReduceHealth(int amount)
    {
        this.health -= amount;
        this.spriteRendrer.sprite = stateSprites[health];

        if (this.health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnMouseDown()
    {
        var distanceToPlayer = Vector2.Distance(
            GameObject.Find("Player").transform.position,
            this.transform.position);

        Debug.Log("Distance to player: " + distanceToPlayer + ". Interact distance: " + this.interactDistance);

        if (distanceToPlayer <= this.interactDistance)
        {
            this.ReduceHealth(1);
        }
    }
}
