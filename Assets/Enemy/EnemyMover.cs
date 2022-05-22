using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] private float _moveSpeed = 1f;
    [SerializeField][Range(0f, 20f)] private float _turnSpeed = 4f;

    private List<Node> _path = new List<Node>();
    private Enemy _enemy;
    private GridManager _gridManager;
    private Pathfinder _pathfinder;

    private void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder= FindObjectOfType<Pathfinder>();
    }

    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = _pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        _path.Clear();
        _path = _pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinates);
    }

    private void FinishPath()
    {
        _enemy.StealGold();
        gameObject.SetActive(false);
    }

    private IEnumerator FollowPath()
    {      
        for (int i = 1; i < _path.Count; i++)
        {
            float travelPercent = 0f;
            Vector3 startPosition = transform.position;
            Vector3 endPosition = _gridManager.GetPositionFromCoordinates(_path[i].Coordinates);

            float rotatePercent = 0f;
            Vector3 targetDirection = (endPosition - transform.position).normalized;
            Quaternion startRotation = transform.rotation;

            // Only attempt rotation if necessary. Avoid "Look Rotation Viewing Vector Is Zero" error.
            if (targetDirection != Vector3.zero)
            {
                Quaternion endRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                while (transform.rotation != endRotation)
                {
                    rotatePercent += Time.deltaTime * _turnSpeed;
                    transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotatePercent);
                    yield return null;
                }
            }


            while (travelPercent < 1)
            {
                travelPercent += Time.deltaTime * _moveSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return null;
            }
            
        }

        FinishPath();
    }
}
