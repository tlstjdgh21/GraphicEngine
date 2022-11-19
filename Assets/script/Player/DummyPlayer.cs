using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    private bool m_isright;

    private void Awake()
    {
        m_isright = true;

    }

   
    // Update is called once per frame
    void Update()
    {
        float playerMove = Input.GetAxisRaw("Horizontal")* 3f *Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerMove = -1.5f * Time.deltaTime;
            m_isright = false;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

            playerMove = 1.5f * Time.deltaTime;
            m_isright = true;
        }
        this.transform.Translate(new Vector3(playerMove, 0, 0));

        if (m_isright) GetComponent<SpriteRenderer>().flipX = false;
        else GetComponent<SpriteRenderer>().flipX = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(1);
    }
}
