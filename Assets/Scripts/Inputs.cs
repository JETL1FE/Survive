using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Inputs Instance { get; private set; }
    private string moveAxisName = "Vertical";
    private string moveHorizonName = "Horizontal";

    public Vector3 mouseWorldPosition;
    public Vector3Int CellIntPosition;
    public Vector3 CellQuaterPosition;
    public Vector3 CellHalfPosition;
    public Vector2 HalfVector;
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool SpaceInput { get; private set; }
    public bool LeftClicked { get; private set; }
    public bool RightClicked { get; private set; }
    public bool RightClickedToggle { get; private set; }
    public bool SpaceToggle { get; private set; }
    public bool XInput { get; private set; }
    public bool CInput { get; private set; }

    public bool QInput { get; private set; }
    public bool RInput { get; private set; }
    public bool EInput { get; private set; }
    public bool AltToggle { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        VerticalInput = Input.GetAxis(moveAxisName);
        HorizontalInput = Input.GetAxis(moveHorizonName);
        LeftClicked = Input.GetMouseButtonDown(0);
        RightClicked = Input.GetMouseButtonDown(1);
        SpaceInput = Input.GetKeyDown(KeyCode.Space);

        CellHalfPosition = new Vector3(
            Mathf.Floor(mouseWorldPosition.x / 0.5f) * 0.5f + 0.25f,
            Mathf.Floor(mouseWorldPosition.y / 0.5f) * 0.5f + 0.25f,
            5);

        HalfVector = new(CellHalfPosition.x, CellHalfPosition.y);

        if (Input.GetKeyDown(KeyCode.Space)) // Test¿ë ÄÚµå
        {
            SpaceToggle = !SpaceToggle;
            if (SpaceToggle)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RightClickedToggle = !RightClickedToggle;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            AltToggle = !AltToggle;
        }
        XInput = Input.GetKeyDown(KeyCode.X);
        QInput = Input.GetKeyDown(KeyCode.Q);
        CInput = Input.GetKeyDown(KeyCode.C);
        RInput = Input.GetKeyDown(KeyCode.R);
        EInput = Input.GetKeyDown(KeyCode.E);

    }
    public void RightToggleReset()
    {
        RightClickedToggle = false;
    }
}
