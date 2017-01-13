using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

	[SerializeField]
	private float m_fPieceSize = 0.2f;

    public enum PieceType
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3
    }

    public GameObject CubePrefab;

    private static Vector2[,] cubePositions =
    {
        {
            new Vector2( 0.5f,    1),
            new Vector2( 0.5f,    0),
            new Vector2(-0.5f,    0),
            new Vector2(-0.5f,   -1)
        },{
            new Vector2(    0, 0.5f),
            new Vector2(   -1,-0.5f),
            new Vector2(    0,-0.5f),
            new Vector2(    1,-0.5f)
        },{
            new Vector2(    0, 1.5f),
            new Vector2(    0, 0.5f),
            new Vector2(    0,-0.5f),
            new Vector2(    0,-1.5f)
        },{
            new Vector2(   -0.5f, 1),
            new Vector2(   -0.5f, 0),
            new Vector2(   -0.5f,-1),
            new Vector2(    0.5f,-1)
        }
    };

    private void addChildren(Vector3 localPosition)
    {
        GameObject obj = Instantiate(CubePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = this.transform;
        obj.transform.localPosition = localPosition;
        print("adding children for piece " +  this);
    }

    private void genPiece(PieceType type)
    {
        for (int i=0; i<4; i++)
        {
            Vector2 xyPos = cubePositions[(int)type, i];
            addChildren(new Vector3(xyPos.x, xyPos.y));
        }
    }

    private PieceType _type;

    public PieceType Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            foreach (Transform child in this.transform)
            {
                Destroy(child);
            }
            genPiece(value);
        }
    }

	void Start () {
        var values = System.Enum.GetValues(typeof(PieceType));
        Type = (PieceType) values.GetValue(Random.Range(0, values.Length));

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localScale = new Vector3 (m_fPieceSize, m_fPieceSize, m_fPieceSize);
	}
}
