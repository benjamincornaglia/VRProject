using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwippingObject : MonoBehaviour {

	public ScoreManager _themanager = null;
	public string ObjectsTag = "PicaVoxelVolume";
    bool _collisioned = false;
    float m_fColTimer = 0f;

    void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == ObjectsTag && _collisioned == false)
		{
			
            _collisioned = true;
		}
        _themanager.swipeHappen(col.contacts[0].point);
    }

	void Start() {
		if (_themanager == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le score manager !");
		}

		if (ObjectsTag == null) {
			throw new System.ArgumentException ("SwipCounter : Merci de renseigner le tag des objets destructibles !");
		}
	}

    void Update()
    {
        if (_collisioned)
            m_fColTimer += Time.deltaTime;
        if (m_fColTimer > 1)
        {
            _collisioned = false;
            m_fColTimer = 0f;
        }
    }
}
