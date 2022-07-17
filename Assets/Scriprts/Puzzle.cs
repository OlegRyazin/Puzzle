using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Camera camera;
    public GameObject[] puzzles;
    public GameObject[] formPuzzles;
    public float correctDistance = 70f;
    bool canMove = true;
    bool puzzleLerp = false;
    public float lerpTime = 1f;

    public void SetPosition(Transform obj)
    {
        if (canMove)
        {
            var mouse = camera.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
            obj.position = mouse;
            obj.SetAsLastSibling();
        }
    }

    public void CheckStatus()
    {
        bool puzzleCheckStatus = true;
        for (int i = 0; i < puzzles.Length; i++)
        {
            if (MagnitudeWithoutSqrt2D(puzzles[i], formPuzzles[i]) > Mathf.Pow(correctDistance, 2))
            {
                puzzleCheckStatus = false;
                break;
            }
        }
        if (puzzleCheckStatus)
        {
            canMove = false;
            puzzleLerp = true;
        } 
    }

    private float MagnitudeWithoutSqrt2D(GameObject puzzle, GameObject formPuzzle)
    {
        float xD = puzzle.transform.localPosition.x - formPuzzle.transform.localPosition.x;
        float yD = puzzle.transform.localPosition.y - formPuzzle.transform.localPosition.y;
        float zD = puzzle.transform.localPosition.z - formPuzzle.transform.localPosition.z;
        return (xD * xD + yD * yD + zD * zD);
    }

    private void Update()
    {
        if(puzzleLerp)
        {
            lerpTime = Mathf.Clamp(lerpTime - Time.deltaTime, 0, 1f);
            for (int i = 0; i < puzzles.Length; i++)
            {
                puzzles[i].transform.localPosition = Vector3.Lerp(puzzles[i].transform.localPosition, formPuzzles[i].transform.localPosition, (1 - lerpTime));
            }
            if (lerpTime == 0)
            {
                for (int i = 0; i < puzzles.Length; i++)
                {
                    puzzles[i].transform.localPosition = formPuzzles[i].transform.localPosition;
                }
                puzzleLerp = false;
            }
        }
    }
}
