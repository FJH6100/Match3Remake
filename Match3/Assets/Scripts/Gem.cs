using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int position;
    [HideInInspector]
    public Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private bool mousePressed;
    private float swipeAngle = 0f;
    private Gem otherGem;
    public enum GemType
    {
        Blue,
        Green,
        Red,
        Yellow,
        Purple
    }
    public GemType type;
    [HideInInspector]
    public bool isMatched;
    [HideInInspector]
    public Vector2Int previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, position) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, position, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(position.x, position.y, 0);
            board.allGems[position.x, position.y] = this;
        }
            
        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
        
    }

    public void SetupGem(int x, int y, Board b)
    {
        position = new Vector2Int(x, y);
        board = b;
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
    }
    
    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        if (Vector3.Distance(firstTouchPosition,finalTouchPosition) > .5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPosition = position;
        if (swipeAngle < 45 && swipeAngle > -45 && position.x < board.width - 1)
        {
            otherGem = board.allGems[position.x + 1, position.y];
            otherGem.position.x--;
            position.x++;
        }
        else if (swipeAngle <= 135 && swipeAngle > 45 && position.y < board.height - 1)
        {
            otherGem = board.allGems[position.x, position.y + 1];
            otherGem.position.y--;
            position.y++;
        }
        else if (swipeAngle >= -135 && swipeAngle < -45 && position.y > 0)
        {
            otherGem = board.allGems[position.x, position.y - 1];
            otherGem.position.y++;
            position.y--;
        }
        else if ((swipeAngle > 135 || swipeAngle < -135) && position.x > 0)
        {
            otherGem = board.allGems[position.x - 1, position.y];
            otherGem.position.x++;
            position.x--;
        }
        board.allGems[position.x, position.y] = this;
        board.allGems[otherGem.position.x, otherGem.position.y] = otherGem;

        StartCoroutine(CheckMove());
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        board.matchFind.FindAllMatches();
        if (otherGem != null)
        {
            if (!isMatched && !otherGem.isMatched)
            {
                otherGem.position = position;
                position = previousPosition;

                board.allGems[position.x, position.y] = this;
                board.allGems[otherGem.position.x, otherGem.position.y] = otherGem;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
