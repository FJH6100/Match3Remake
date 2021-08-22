using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject backgroundTile;
    public Gem[] gems;
    [HideInInspector]
    public Gem[,] allGems;
    public float gemSpeed;
    [HideInInspector]
    public MatchFinder matchFind;

    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
    }
    // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width,height];
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
                while (MatchesAt(new Vector2Int(i,j), gems[gemIndex]))
                {
                    gemIndex = Random.Range(0, gems.Length);
                }
                SpawnGem(gems[gemIndex], i, j);
            }
        }
    }

    private void Update()
    {
        matchFind.FindAllMatches();
    }

    private void SpawnGem(Gem g, int x, int y)
    {
        Gem gem = Instantiate(g, new Vector3(x, y, 0), Quaternion.identity);
        gem.transform.parent = transform;
        allGems[x, y] = gem;
        gem.SetupGem(x, y, this);
    }

    bool MatchesAt(Vector2Int posToCheck, Gem gemtoCheck)
    {
        if (posToCheck.x > 1)
        {
            if (allGems[posToCheck.x - 1, posToCheck.y].type == gemtoCheck.type && allGems[posToCheck.x - 2, posToCheck.y].type == gemtoCheck.type)
                return true;
        }
        if (posToCheck.y > 1)
        {
            if (allGems[posToCheck.x, posToCheck.y - 1].type == gemtoCheck.type && allGems[posToCheck.x, posToCheck.y - 2].type == gemtoCheck.type)
                return true;
        }
        return false;
    }
    
    private void DestroyAt(Vector2Int pos)
    {
        if (allGems[pos.x,pos.y] != null)
        {
            Destroy(allGems[pos.x, pos.y].gameObject);
            allGems[pos.x, pos.y] = null;
        }
    }

    public void DestroyMatches()
    {
        foreach (Gem g in matchFind.matched)
        {
            if (g != null)
                DestroyAt(g.position);
        }
        StartCoroutine(MoveGemsDown());
    }
    private IEnumerator MoveGemsDown()
    {
        yield return new WaitForSeconds(.2f);
        for (int x = 0; x < width; x++)
        {
            int gaps = 0;
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                    gaps++;
                else if (gaps > 0)
                {
                    allGems[x, y].position.y -= gaps;
                    allGems[x, y - gaps] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
        }
        StartCoroutine(FillBoard());
    }

    private IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();
    }

    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemIndex = Random.Range(0, gems.Length);
                    SpawnGem(gems[gemIndex], x, y);
                }
            }
        }
    }
}
