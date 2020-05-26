using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWalls : DCLSingletonBase<AirWalls>
{
	public Transform airWallPrefeb;
	public Transform airWalls;

	void Start()
    {
	}

	public void Generate()
    {
        Helper.ClearAllChild(transform);
        var sideWidth = Ground.I.sideWidth;

        // Set air walls (4-direction)
        // `sideWidth / 10 + 2` means (`N` of grassBlocks on a side) + 2
        airWallPrefeb.localScale = new Vector3(sideWidth / 10 + 2, 1, 1);
        float[,] positions = new float[,]
        {
            { 0f, sideWidth / 2  - .5f },
            { sideWidth / 2 - .5f, 0f },
            { 0f, -sideWidth / 2 + .5f },
            { -sideWidth / 2 + .5f, 0f },
        };
        for (int dirt = 0; dirt < 4; dirt++)
        {
            Instantiate(airWallPrefeb, new Vector3(positions[dirt, 0], 3f, positions[dirt, 1]), Quaternion.Euler(-90f, 0, dirt * 90f), airWalls);
        }
	}
}
