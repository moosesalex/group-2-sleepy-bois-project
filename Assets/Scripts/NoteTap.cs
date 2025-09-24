using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTap : Note
{
    public override void UpdateGameObject()
    {
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = GetNotePosition(t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public override void Hit()
    {
        Destroy(gameObject);
    }
    public override void Miss()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}
