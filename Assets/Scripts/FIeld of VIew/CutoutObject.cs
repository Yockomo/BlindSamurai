using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float cutoutSize;
    [SerializeField] private float falloffSize;

    [Header("References")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;

    private Camera mainCamera;

    private readonly string CutoutPosition = "_CutoutPosition";
    private readonly string CutoutSize = "_CutoutSize";
    private readonly string FalloffSize = "_FalloutSize";
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        var cutoutPosition = mainCamera.WorldToViewportPoint(targetObject.transform.position);
        // cutoutPosition.y /= (Screen.width / Screen.height);

        Vector2 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        Debug.Log(cutoutPosition);
        
        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int j = 0; j < materials.Length; ++j)
            {
                materials[j].SetVector(CutoutPosition,cutoutPosition);
                materials[j].SetFloat(CutoutSize, cutoutSize);
                materials[j].SetFloat(FalloffSize, falloffSize);
            }
        }
    }
    
    // private void GetCutoutObjects()
    // {
    //     Vector2 offset = targetObject.position - transform.position;
    //     Physics.RaycastNonAlloc(transform.position, offset, hitObjects, offset.magnitude,wallMask);
    // }
    //
    // private void Cutout()
    // {
    //     var cutoutPosition = GetCutoutPosition();
    //     
    //     for (int i = 0; i < hitObjects.Length; ++i)
    //     {
    //         Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
    //
    //         for (int j = 0; j < materials.Length; ++j)
    //         {
    //             materials[j].SetVector(CutoutPosition,cutoutPosition);
    //             materials[j].SetFloat(CutoutSize, cutoutSize);
    //             materials[j].SetFloat(FalloffSize, falloffSize);
    //         }
    //     }
    // }
    
    private Vector3 GetCutoutPosition()
    {
        var cutoutPosition = mainCamera.WorldToViewportPoint(targetObject.transform.position);
        cutoutPosition.y /= (Screen.width / Screen.height);
        return cutoutPosition;
    }
}
