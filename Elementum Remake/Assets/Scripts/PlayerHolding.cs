using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolding : MonoBehaviour
{
    public GameObject holding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (holding != null)
        {
            Key key = holding.GetComponent<Key>();
            if (collision.gameObject == key.door)
            {
                key.Activate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (holding != null)
        {
            holding.transform.position = new Vector2(transform.position.x, transform.position.y + 1.5f);
        }
    }

    
}
