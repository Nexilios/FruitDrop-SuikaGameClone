using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class FruitScript : MonoBehaviour
{
    public FruitData fruit_data;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fruit"))
        {
            FruitScript otherFruit = other.gameObject.GetComponent<FruitScript>();

            /*if (otherFruit != null && otherFruit.fruitName == fruitName &&
                gameObject.GetInstanceID() < otherFruit.GetInstanceID())
            {
                MergeFruit(other.gameObject);
            }*/
        }
    }

    /*private void MergeFruit(GameObject otherFruit)
    {
        if (upgradeFruitPrefab == null) return;
        
        // New position will be in between the 2 merging fruits
        Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;
        
        Instantiate(upgradeFruitPrefab, mergePosition, Quaternion.identity);
        
        // Destroy the merged fruit
        Destroy(gameObject);
        Destroy(otherFruit);
    }*/
}
