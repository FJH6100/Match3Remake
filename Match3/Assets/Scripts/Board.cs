using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject backgroundTile;
    public Gem[] gems;
    Gem[,] gemLayout;
    // Start is called before the first frame update
    void Start()
    {
        gemLayout = new Gem[width,height];
        Setup();
    }

    private void Setup()
    {
        Camera.main.transform.position = new Vector3(width * .5f - .5f, height * .5f - .5f, -10);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject bg = Instantiate(backgroundTile, new Vector2(i, j), Quaternion.identity);
                bg.transform.parent = transform;
                bg.name = "Tile " + i + " " + j;
                int gemIndex = Random.Range(0, gems.Length);
                Gem gemToUse = gems[gemIndex];
                SpawnGem(gemToUse, i, j);
            }
        }
    }

    private void SpawnGem(Gem g, int x, int y)
    {
        Gem gem = Instantiate(g, new Vector3(x, y, 0), Quaternion.identity);
        gem.transform.parent = transform;
        gemLayout[x, y] = gem;
        gem.SetupGem(x, y, this);
    }
}
