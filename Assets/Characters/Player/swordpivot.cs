using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordpivot : MonoBehaviour {
    private PlayerCharacter player;

	// Use this for initialization
	void Start () {
        player = this.GetComponentInParent<PlayerCharacter>();
	}

    public void OnEndAnimation(string label)
    {
        if (label == "Attack")
        {
            player.attacking = false;
        }
    }
}
