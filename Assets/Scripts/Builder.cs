//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Unity.Mathematics;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.EventSystems;

//public class Builder : MonoBehaviour
//{
//    public static Builder Instance { get; private set; }
//    [SerializeField]
//    [Header("Wall Prefabs Data")]
//    public GameObject[] wallPrefabs;
//    [Header("Construct")]
//    [SerializeField]
//    private GameObject selectedObject;
//    [SerializeField]
//    private GameObject constructionPreviewObject = null;
//    [SerializeField]
//    private bool constructionPossible;
//    Vector2 inspectionSize;
//    public List<GameObject> builtObjects;
//    public List<Companion> companions;
//    public Transform canvasTransform;
//    public GameObject wallHpSliderPrefab;
//    public float distanceThresholdForConstruction = 1.5f;
//    [SerializeField]
//    private GameObject OnClickObject = null;
//    [SerializeField]
//    private bool isBuilding = false;
//    [SerializeField]
//    private bool isRepair = false;
//    public GameObject mouseCursor;
//    public ActionMenu actionMenu;
//    private Player player;
//    private Vector2 MouseToVector;
//    [SerializeField]
//    private bool ignoreClick = false;
//    public int obstacleLayer;
//    [SerializeField]
//    private float clickIgnoreDuration = 0.1f;
//    private bool ignoreRepaircall = false;
//    [SerializeField]
//    private float repairIgnoreDuration = 0.7f;

//    public int curpopulation = 0;
//    public int populationLimit = 8;
//    [SerializeField]
//    private int prefabIndex;

//    public int barrackLimit;
//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//        obstacleLayer = (1 << LayerMask.NameToLayer("Monster") |
//            (1 << LayerMask.NameToLayer("Ally")) |
//            1 << LayerMask.NameToLayer("Obstacle"));
//    }
//    void SpawnAllyHpSlider(GameObject _ally)
//    {
//        GameObject sliderClone = Instantiate(wallHpSliderPrefab);
//        sliderClone.transform.SetParent(canvasTransform);
//        sliderClone.transform.localScale = Vector3.one;
//        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(_ally.transform);
//        sliderClone.GetComponent<HpViewer>().SetUp(_ally.GetComponent<LivingEntity>());
//    }
//    private void Start()
//    {
//        player = GameManager.Instance.PlayerGetter();
//        if (player)
//        {
//            SpawnAllyHpSlider(player.gameObject);
//        }
//        actionMenu.gameObject.SetActive(false);
//    }

//    private void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.Space))
//        {
//            CountCompanion();

//        }
//        MouseToVector = Inputs.Instance.HalfVector;
//        if (selectedObject)
//        {
//            selectedObject.transform.position = MouseToVector;
//            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(MouseToVector, inspectionSize, 0, obstacleLayer);
//            constructionPossible = collider2Ds.Length == 0;
//            selectedObject.GetComponent<LivingEntity>().StateBool("CANT", !constructionPossible);
//            selectedObject.GetComponent<LivingEntity>().Body.sortingOrder = 999;
//            if (Inputs.Instance.LeftClicked && !EventSystem.current.IsPointerOverGameObject() && !ActionMenu.IsActive() && constructionPossible && !ignoreClick)
//            {
//                if (!constructionPreviewObject)
//                {
//                    constructionPreviewObject = Instantiate(wallPrefabs[prefabIndex], MouseToVector, Quaternion.identity);
//                    constructionPreviewObject.layer = LayerMask.NameToLayer("Temp");
//                    StartCoroutine(MoveToBuild());
//                }
//            }

//        }
//        else
//        {
//            Destroy(constructionPreviewObject);
//            constructionPreviewObject = null;
//            if (Inputs.Instance.LeftClicked && !EventSystem.current.IsPointerOverGameObject() && !ActionMenu.IsActive())
//            {
//                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0, obstacleLayer);
//                if (hit.collider != null)
//                {
//                    OnClickObject = hit.collider.gameObject;
//                    //Debug.Log(hit.collider.name);
//                    OnClickMenu.ArgSetter(OnClickObject);
//                }
//                else
//                {
//                    player.MoveTo(MouseToVector);
//                    OnClickMenu.ArgSetter(null);
//                    OnClickObject = null;
//                }
//            }
//        }
//        if (Inputs.Instance.RightClicked)
//        {
//            if (ActionMenu.IsActive() == false)
//            {
//                if (selectedObject)
//                {
//                    Destroy(selectedObject);
//                    selectedObject = null;
//                }
//            }
//            if (selectedObject == null)
//            {
//                ActionMenu.Active(!ActionMenu.IsActive());
//            }
//            StopCoroutine(MoveToBuild());
//            player.StopMove();
//        }
//        if (Input.GetKeyDown(KeyCode.LeftAlt))
//        {
//            canvasTransform.gameObject.SetActive(!canvasTransform.gameObject.activeSelf);
//        }
//    }
//    public void OnClickToRepair()
//    {
//        StartCoroutine(MoveToRepair());
//    }

//    public IEnumerator MoveToBuild()
//    {
//        if (constructionPreviewObject)
//        {
//            isBuilding = true;
//            while (isBuilding)
//            {
//                if (constructionPreviewObject)
//                {
//                    if (Vector2.Distance(constructionPreviewObject.transform.position, player.transform.position) > distanceThresholdForConstruction)
//                    {
//                        player.MoveTo(constructionPreviewObject.transform.position);
//                        //player.agent.SetDestination(constructionPreviewObject.transform.position);
//                        yield return null;
//                    }
//                    else
//                    {
//                        player.StopMove();
//                        player.LookAt(constructionPreviewObject.transform.position);
//                        player.Trigger("Slash");
//                        yield return StartCoroutine(player.CheckAnimationState("Slash"));
//                        // Null 체크 추가
//                        if (constructionPreviewObject)
//                        {
//                            BuildWall(wallPrefabs[prefabIndex]);

//                        }
//                    }
//                }
//                else
//                {
//                    isBuilding = false;
//                    break;
//                }
//            }
//        }
//        ignoreClick = true;
//        yield return new WaitForSecondsRealtime(clickIgnoreDuration);
//        ignoreClick = false;
//    }
//    public void BuildWall(GameObject temp)
//    {
//        if (temp != null)
//        {
//            if (temp.GetComponent<WallData>())
//            {
//                WallData wallData = temp.GetComponent<WallData>();
//                if (wallData.CostGetter() <= GameManager.Instance.CurrentGoldGetter())
//                {
//                    GameObject tempWallObject = Instantiate(temp, constructionPreviewObject.transform.position, Quaternion.identity);
//                    Destroy(constructionPreviewObject);
//                    constructionPreviewObject = null;
//                    builtObjects.Add(tempWallObject);
//                    tempWallObject.name = $"{temp.gameObject.name}";
//                    tempWallObject.GetComponent<WallData>().SetUp();
//                    tempWallObject.GetComponent<WallData>().Trigger("Brr");
//                    SpawnAllyHpSlider(tempWallObject);
//                    Instantiate(GameManager.Instance.MakeEffect, tempWallObject.transform.position, Quaternion.identity);
//                    GameManager.Instance.SpendGold(wallData.CostGetter());
//                    Debug.Log("BuildWall 완료");
//                    isBuilding = false;

//                    if(tempWallObject.gameObject.GetComponent<Barracks>())
//                    {
//                        Destroy(selectedObject);
//                        selectedObject = null;
//                    }
//                }
//                else
//                {
//                    Debug.Log("Need More Gold");
//                }
//            }
//            else
//            {
//                Debug.Log("Temp Not Have Walldata");
//            }
//        }
//    }


//    private void RepairWall(LivingEntity temp, int amount)
//    {
//        temp.RestoreHealth(amount);
//        GameManager.Instance.SpendGold(amount);
//    }
//    public IEnumerator MoveToRepair()
//    {
//        if (OnClickObject != null && OnClickObject.GetComponent<WallData>())
//        {
//            ignoreRepaircall = true;
//            WallData tempEntity = OnClickObject.GetComponent<WallData>();
//            if (tempEntity != null && tempEntity.Health < tempEntity.ReturnCurMax())
//            {
//                isRepair = true;
//                while (isRepair)
//                {
//                    if (Vector2.Distance(tempEntity.transform.position, player.transform.position) > distanceThresholdForConstruction)
//                    {
//                        player.MoveTo(tempEntity.transform.position);
//                        yield return null;
//                    }
//                    else
//                    {
//                        player.StopMove();
//                        int goldcal = tempEntity.MaxHealth - tempEntity.Health;
//                        if (goldcal < GameManager.Instance.CurrentGoldGetter())
//                        {
//                            player.Trigger("Slash");
//                            yield return StartCoroutine(player.CheckAnimationState("Slash"));
//                            if (tempEntity != null && tempEntity.gameObject != null)
//                            {
//                                RepairWall(tempEntity, goldcal);
//                            }
//                        }
//                        else
//                        {
//                            PopUpText.DisplayText("골드가 모자라요");
//                        }
//                        isRepair = false;
//                    }
//                }
//            }
//        }
//        //else
//        //{
//        //    StopCoroutine(MoveToRepair());
//        //}
//        ignoreRepaircall = true;
//        yield return new WaitForSeconds(repairIgnoreDuration);
//        ignoreRepaircall = false;
//    }
//    public void Selecter(int index)
//    {
//        if (wallPrefabs.Length > 0)
//        {
//            prefabIndex = index;
//            selectedObject = Instantiate(wallPrefabs[index]);
//            selectedObject.name = $"Preview / {index}";
//            //selectedObject.GetComponent<Collider2D>().enabled = false;
//            Collider2D[] colliders = selectedObject.GetComponents<Collider2D>();
//            foreach (Collider2D collider in colliders)
//            {
//                collider.enabled = false;
//            }

//            if (selectedObject.GetComponent<GoldMine>())
//            {
//                selectedObject.GetComponent<GoldMine>().enabled = false;
//            }
//            //selectedObject.GetComponent<LivingEntity>().Body.sortingOrder4
//            selectedObject.GetComponent<NavMeshObstacle>().enabled = false;
//            inspectionSize = new(selectedObject.GetComponent<BoxCollider2D>().size.x - 0.05f, selectedObject.GetComponent<BoxCollider2D>().size.y - 0.05f);
//        }
//        else { }
//    }

//    public void CountCompanion()
//    {
//        // companions 리스트에서 각 Companion의 이름을 추출하여 그룹화합니다.
//        var groupedCompanions = companions.GroupBy(companion => companion.name);

//        // 그룹화된 리스트 중에서 요소의 개수가 2개 이상인 그룹이 있는지 확인합니다.
//        var duplicatedCompanions = groupedCompanions.Where(group => group.Count() > 1);

//        // 요소의 개수가 2개 이상인 그룹이 있다면 해당 Companion 이름을 출력합니다.
//        foreach (var group in duplicatedCompanions)
//        {
//            Debug.Log($"combine 가능한 오브젝트 : {group.Key}");
//            PopUpText.DisplayText($"{group.Key}");
//        }

//    }
//    public bool IsDuplicateCompanion(Companion _companion)
//    {
//        var duplicatedCompanion = companions.FirstOrDefault(c => c.name == _companion.name && c != _companion);
//        Debug.Log($"{_companion.name}");
//        return duplicatedCompanion != null;
//    }
//    public void Test()
//    {
//        //companions.Distinct<Companion>;
//    }

//    private void OnApplicationQuit()
//    {
//        companions.Clear();
//        builtObjects.Clear();
//    }
//    private void OnDisable()
//    {
//        Debug.Log("!!!OnDisable");
//        //companions.Clear();
//        //companions = null;
//        //builtObjects.Clear();
//        //builtObjects = null;
//    }
//    private void OnDestroy()
//    {
//        Debug.Log("!!!OnDestroy");
//        companions.Clear();
//        companions = null;
//        builtObjects.Clear();
//        builtObjects = null;
//    }
//    public void CheckDuplicateCompanion(Companion companion)
//    {
//        bool isDuplicate = IsDuplicateCompanion(companion);
//        Debug.Log("Is Duplicate Companion: " + isDuplicate);
//    }
//    public void CompanionCombiner(Companion _companion)
//    {
//        if((int)_companion.grade >= _companion.GradeLimitInt-1)
//        {
//            PopUpText.DisplayText("최고등급입니다.");
//        }
//        else
//        {

//            List<Companion> sameNameCompanions = companions.FindAll(c => c.name == _companion.name && c != _companion);

//            if (sameNameCompanions.Count == 0)
//            {
//                Debug.Log("동일한 요소가 없음");
//                return;
//            }

//            Companion closestCompanion = null;
//            float closestDistance = Mathf.Infinity;
//            foreach (Companion companion in sameNameCompanions)
//            {
//                float distance = Vector3.Distance(_companion.transform.position, companion.transform.position);
//                if (distance < closestDistance)
//                {
//                    closestCompanion = companion;
//                    closestDistance = distance;
//                }
//            }

//            if (closestCompanion != null)
//            {
//                Debug.Log($"가장 가까운 요소: {closestCompanion.name} and {closestCompanion.transform.position} \n {_companion.name} {_companion.transform.position}");
//                //companions.Remove(_companion);
//                _companion.GradeUp();
//                GameObject tempParticle = Instantiate(GameManager.Instance.boomEffect, _companion.transform.position, quaternion.identity);
//                companions.Remove(closestCompanion);
//                Destroy(closestCompanion.gameObject);
//            }
//            else
//            {
//                Debug.Log("가까운 원소가 없음");
//            }
//        }
//    }

//    public int CountWallBoxes()
//    {
//        int count = 0;
//        foreach (GameObject obj in builtObjects)
//        {
//            if (obj != null && obj.name == "WallBox") // obj가 null이 아니고 이름이 "WallBox"인지 확인
//            {
//                count++;
//            }
//        }
//        return count;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    public static Builder Instance { get; private set; }

    [Header("Wall Prefabs Data")]
    public GameObject[] wallPrefabs;

    [Header("Construct")]
    [SerializeField]
    private GameObject selectedObject;
    [SerializeField]
    private GameObject constructionPreviewObject = null;
    [SerializeField]
    private bool constructionPossible;

    private Vector2 inspectionSize;
    public List<GameObject> builtObjects;
    public List<Companion> companions;
    public Transform canvasTransform;
    public GameObject wallHpSliderPrefab;
    public float distanceThresholdForConstruction = 1.5f;

    [SerializeField]
    private GameObject OnClickObject = null;
    [SerializeField]
    private bool isBuilding = false;
    [SerializeField]
    private bool isRepair = false;

    public GameObject mouseCursor;
    public ActionMenu actionMenu;
    private Player player;
    private Vector2 MouseToVector;
    [SerializeField]
    private bool ignoreClick = false;

    public int obstacleLayer;
    [SerializeField]
    private float clickIgnoreDuration = 0.1f;
    //private bool ignoreRepaircall = false;
    //[SerializeField]
    //private float repairIgnoreDuration = 0.7f;

    public int curpopulation = 0;
    public int populationLimit = 8;
    [SerializeField]
    private int prefabIndex;
    public int barrackLimit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 싱글톤 패턴: 기존 인스턴스가 있다면 새로 생성된 인스턴스를 파괴
        }

        obstacleLayer = (1 << LayerMask.NameToLayer("Monster") |
                         (1 << LayerMask.NameToLayer("Ally")) |
                         1 << LayerMask.NameToLayer("Obstacle")); // 레이어 설정을 비트 시프트 연산으로 처리
    }

    void SpawnAllyHpSlider(GameObject _ally)
    {
        GameObject sliderClone = Instantiate(wallHpSliderPrefab);
        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;
        sliderClone.GetComponent<SliderPositionAutoSetter>().SetUp(_ally.transform);
        sliderClone.GetComponent<HpViewer>().SetUp(_ally.GetComponent<LivingEntity>());
    }

    private void Start()
    {
        player = GameManager.Instance.PlayerGetter();
        if (player)
        {
            SpawnAllyHpSlider(player.gameObject);
        }
        actionMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CountCompanion();
        }

        MouseToVector = Inputs.Instance.HalfVector;

        if (selectedObject)
        {
            selectedObject.transform.position = MouseToVector;
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(MouseToVector, inspectionSize, 0, obstacleLayer);
            constructionPossible = collider2Ds.Length == 0;
            selectedObject.GetComponent<LivingEntity>().StateBool("CANT", !constructionPossible);
            selectedObject.GetComponent<LivingEntity>().Body.sortingOrder = 999;

            if (Inputs.Instance.LeftClicked &&
                !EventSystem.current.IsPointerOverGameObject() &&
                !ActionMenu.IsActive() &&
                constructionPossible &&
                !ignoreClick)
            {
                if (!constructionPreviewObject)
                {
                    constructionPreviewObject = Instantiate(wallPrefabs[prefabIndex], MouseToVector, Quaternion.identity);
                    constructionPreviewObject.layer = LayerMask.NameToLayer("Temp");
                    StartCoroutine(MoveToBuild());
                }
            }
        }
        else
        {
            Destroy(constructionPreviewObject);
            constructionPreviewObject = null;

            if (Inputs.Instance.LeftClicked &&
                !EventSystem.current.IsPointerOverGameObject() &&
                !ActionMenu.IsActive())
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0, obstacleLayer);
                if (hit.collider != null)
                {
                    OnClickObject = hit.collider.gameObject;
                    // Debug.Log(hit.collider.name);
                    OnClickMenu.ArgSetter(OnClickObject);
                }
                else
                {
                    player.MoveTo(MouseToVector);
                    OnClickMenu.ArgSetter(null);
                    OnClickObject = null;
                }
            }
        }

        if (Inputs.Instance.RightClicked)
        {
            if (selectedObject)
            {
                Destroy(selectedObject);
                selectedObject = null;
                StopCoroutine(MoveToBuild());
                player.StopMove();
            }
            else
            {
                ActionMenu.Active(!ActionMenu.IsActive());
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            canvasTransform.gameObject.SetActive(!canvasTransform.gameObject.activeSelf);
        }
    }


    //public void OnClickToRepair()
    //{
    //    StartCoroutine(MoveToRepair());
    //}

    public IEnumerator MoveToBuild()
    {
        if (constructionPreviewObject)
        {
            isBuilding = true;
            while (isBuilding)
            {
                if (constructionPreviewObject)
                {
                    if (Vector2.Distance(constructionPreviewObject.transform.position, player.transform.position) > distanceThresholdForConstruction)
                    {
                        player.MoveTo(constructionPreviewObject.transform.position);
                        yield return null;
                    }
                    else
                    {
                        player.StopMove();
                        player.LookAt(constructionPreviewObject.transform.position);
                        player.Trigger("Slash");
                        yield return StartCoroutine(player.CheckAnimationState("Slash"));

                        // Null 체크 추가
                        if (constructionPreviewObject)
                        {
                            BuildWall(wallPrefabs[prefabIndex]);
                        }
                    }
                }
                else
                {
                    isBuilding = false;
                    break;
                }
            }
        }
        ignoreClick = true;
        yield return new WaitForSecondsRealtime(clickIgnoreDuration);
        ignoreClick = false;
    }

    public void BuildWall(GameObject temp)
    {
        if (temp != null)
        {
            WallData wallData = temp.GetComponent<WallData>();
            if (wallData)
            {
                if (wallData.CostGetter() <= GameManager.Instance.CurrentGoldGetter())
                {
                    GameObject tempWallObject = Instantiate(temp, constructionPreviewObject.transform.position, Quaternion.identity);
                    Destroy(constructionPreviewObject);
                    constructionPreviewObject = null;
                    builtObjects.Add(tempWallObject);
                    tempWallObject.name = $"{temp.gameObject.name}";
                    tempWallObject.GetComponent<WallData>().SetUp();
                    tempWallObject.GetComponent<WallData>().Trigger("Brr");
                    SpawnAllyHpSlider(tempWallObject);
                    Instantiate(GameManager.Instance.MakeEffect, tempWallObject.transform.position, Quaternion.identity);
                    GameManager.Instance.SpendGold(wallData.CostGetter());
                    Debug.Log("BuildWall 완료");
                    isBuilding = false;

                    if (tempWallObject.gameObject.GetComponent<Barracks>())
                    {
                        Destroy(selectedObject);
                        selectedObject = null;
                    }
                }
                else
                {
                    Debug.Log("Need More Gold");
                }
            }
            else
            {
                Debug.Log("Temp Not Have Walldata");
            }
        }
    }

    //private void RepairWall(LivingEntity temp, int amount)
    //{
    //    temp.RestoreHealth(amount);
    //    GameManager.Instance.SpendGold(amount);
    //}

    //public IEnumerator MoveToRepair()
    //{
    //    if (OnClickObject != null && OnClickObject.GetComponent<WallData>())
    //    {
    //        ignoreRepaircall = true;
    //        WallData tempEntity = OnClickObject.GetComponent<WallData>();
    //        if (tempEntity != null && tempEntity.Health < tempEntity.ReturnCurMax())
    //        {
    //            isRepair = true;
    //            while (isRepair)
    //            {
    //                if (Vector2.Distance(tempEntity.transform.position, player.transform.position) > distanceThresholdForConstruction)
    //                {
    //                    player.MoveTo(tempEntity.transform.position);
    //                    yield return null;
    //                }
    //                else
    //                {
    //                    player.StopMove();
    //                    int goldcal = tempEntity.MaxHealth - tempEntity.Health;
    //                    if (goldcal < GameManager.Instance.CurrentGoldGetter())
    //                    {
    //                        player.Trigger("Slash");
    //                        yield return StartCoroutine(player.CheckAnimationState("Slash"));
    //                        if (tempEntity != null)
    //                        {
    //                            RepairWall(tempEntity, goldcal);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        PopUpText.DisplayText("골드가 모자라요");
    //                    }
    //                    isRepair = false;
    //                }
    //            }
    //        }
    //    }
    //    ignoreRepaircall = true;
    //    yield return new WaitForSeconds(repairIgnoreDuration);
    //    ignoreRepaircall = false;
    //}

    public void Selecter(int index)
    {
        if (wallPrefabs.Length > 0)
        {
            prefabIndex = index;
            selectedObject = Instantiate(wallPrefabs[index]);
            selectedObject.name = $"Preview / {index}";

            Collider2D[] colliders = selectedObject.GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            if (selectedObject.GetComponent<GoldMine>())
            {
                selectedObject.GetComponent<GoldMine>().enabled = false;
            }

            selectedObject.GetComponent<NavMeshObstacle>().enabled = false;
            inspectionSize = new Vector2(
                selectedObject.GetComponent<BoxCollider2D>().size.x - 0.05f,
                selectedObject.GetComponent<BoxCollider2D>().size.y - 0.05f
            );
        }
    }

    public void CountCompanion()
    {
        var groupedCompanions = companions.GroupBy(companion => companion.name);
        var duplicatedCompanions = groupedCompanions.Where(group => group.Count() > 1);

        foreach (var group in duplicatedCompanions)
        {
            Debug.Log($"combine 가능한 오브젝트 : {group.Key}");
            PopUpText.DisplayText($"{group.Key}");
        }
    }

    public bool IsDuplicateCompanion(Companion _companion)
    {
        var duplicatedCompanion = companions.FirstOrDefault(c => c.name == _companion.name && c != _companion);
        Debug.Log($"{_companion.name}");
        return duplicatedCompanion != null;
    }

    public void Test()
    {
        // 미사용 함수, 필요시 추가 로직 구현
    }

    private void OnApplicationQuit()
    {
        companions.Clear();
        builtObjects.Clear();
    }

    private void OnDisable()
    {
        Debug.Log("!!!OnDisable");
    }

    private void OnDestroy()
    {
        Debug.Log("!!!OnDestroy");
        companions.Clear();
        companions = null;
        builtObjects.Clear();
        builtObjects = null;
    }

    public void CheckDuplicateCompanion(Companion companion)
    {
        bool isDuplicate = IsDuplicateCompanion(companion);
        Debug.Log("Is Duplicate Companion: " + isDuplicate);
    }

    public void CompanionCombiner(Companion _companion)
    {
        if ((int)_companion.grade >= _companion.GradeLimitInt - 1)
        {
            PopUpText.DisplayText("최고등급입니다.");
        }
        else
        {
            List<Companion> sameNameCompanions = companions.FindAll(c => c.name == _companion.name && c != _companion);

            if (sameNameCompanions.Count == 0)
            {
                Debug.Log("동일한 요소가 없음");
                return;
            }

            Companion closestCompanion = null;
            float closestDistance = Mathf.Infinity;
            foreach (Companion companion in sameNameCompanions)
            {
                float distance = Vector3.Distance(_companion.transform.position, companion.transform.position);
                if (distance < closestDistance)
                {
                    closestCompanion = companion;
                    closestDistance = distance;
                }
            }

            if (closestCompanion != null)
            {
                Debug.Log($"가장 가까운 요소: {closestCompanion.name} and {closestCompanion.transform.position} \n {_companion.name} {_companion.transform.position}");
                _companion.GradeUp();
                GameObject tempParticle = Instantiate(GameManager.Instance.boomEffect, _companion.transform.position, quaternion.identity);
                companions.Remove(closestCompanion);
                Destroy(closestCompanion.gameObject);
            }
            else
            {
                Debug.Log("가까운 원소가 없음");
            }
        }
    }

    public int CountWallBoxes()
    {
        return builtObjects.Count(obj => obj != null && obj.name == "WallBox");
    }
}