using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethAnimationsPlayer : MonoBehaviour {

    public GameObject Teeths;

	public void Open()
    {
        Animator anim = Teeths.GetComponent<Animator>();
        anim.Play("TeethOpenClose");
    }
	
}
