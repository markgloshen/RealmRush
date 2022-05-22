using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _blockedColor = Color.grey;
    [SerializeField] private Color _exploredColor = Color.yellow;
    [SerializeField] private Color _pathColor = new Color(1f, 0.5f, 0f);
    
    private TextMeshPro _label;
    private Vector2Int _coordinates = new Vector2Int();
    private GridManager _gridManager;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _label = GetComponent<TextMeshPro>();
        _label.enabled = false;

        DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            _label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _label.enabled = !_label.enabled;
        }
    }
    private void SetLabelColor()
    {
        if (_gridManager == null) { return; }

        Node node = _gridManager.GetNode(_coordinates);

        if (node == null) { return; }
        
        if (!node.IsWalkable)
        {
            _label.color = _blockedColor;
        }
        else if (node.IsPath)
        {
            _label.color = _pathColor;
        }
        else if (node.IsExplored)
        {
            _label.color = _exploredColor;
        }
        else
        {
            _label.color = _defaultColor;
        }
    }

    private void DisplayCoordinates()
    {
        if (_gridManager == null ) { return; }

        _coordinates.x = Mathf.RoundToInt(transform.parent.position.x / _gridManager.UnityGridSize);
        _coordinates.y = Mathf.RoundToInt(transform.parent.position.z / _gridManager.UnityGridSize);
        _label.text = _coordinates.x + "," + _coordinates.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = _coordinates.ToString();
    }
}