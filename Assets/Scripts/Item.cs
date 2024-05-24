using UnityEngine;

public class Item : MonoBehaviour
{

    public enum ItemType { Exp, Gold, Health }
    public ItemType Type;
    public int Value;
    //public string Name;   
    private SpriteRenderer Renderer;
    public GameObject gainEffect;
    //[Range(0f, 1f)]
    //public float dropProbability;
    // Start is called before the first frame update
    //public GameObject healEffect;
    //public GameObject goldEffect;
    //public GameObject expEffect;
    //private Rigidbody2D rb;
    //public float speed = 3f;
    void Start()
    {
        Renderer = gameObject.GetComponent<SpriteRenderer>();
        Renderer.sortingOrder = Mathf.RoundToInt(transform.position.y) * -1;
        this.name = Type.ToString();
        Invoke(nameof(DestroySelf), 30f);
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            switch (Type)
            {
                case ItemType.Exp:
                    GameManager.Instance.GainExp(Value);
                    //expEffect = Instantiate(expEffect, collision.transform.position, Quaternion.identity);
                    break;
                case ItemType.Gold:
                    GameManager.Instance.GainGold(Value);
                    //goldEffect = Instantiate(goldEffect, collision.transform.position, Quaternion.identity);
                    break;
                case ItemType.Health:

                    collision.gameObject.GetComponent<Player>().RestoreHealth(Value);
                    //healEffect = Instantiate(healEffect, collision.transform.position, Quaternion.identity);
                    break;
            }

            Instantiate(gainEffect, collision.transform);
            Destroy(gameObject);
        }



    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        switch (Type)
    //        {
    //            case ItemType.Exp:
    //                GameManager.Instance.currentExp += Value;
    //                expEffect = Instantiate(expEffect, collision.transform.position, Quaternion.identity);
    //                break;
    //            case ItemType.Gold:
    //                GameManager.Instance.CurrentGoldGetter() += Value;
    //                goldEffect = Instantiate(goldEffect, collision.transform.position, Quaternion.identity);
    //                break;
    //            case ItemType.Health:
    //                if (GameManager.Instance.IsAlive)
    //                {
    //                    collision.gameObject.GetComponent<PlayerMovement>().RestoreHealth(Value);
    //                    healEffect = Instantiate(healEffect, collision.transform.position, Quaternion.identity);
    //                }
    //                else
    //                {
    //                    return;
    //                }
    //                break;
    //        }
    //        Destroy(gameObject);
    //    }

    //}
    public SpriteRenderer ReturnSpriteRender()
    {
        return Renderer;
    }
}
