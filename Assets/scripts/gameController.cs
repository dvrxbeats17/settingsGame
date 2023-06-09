using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class gameController : MonoBehaviour
{
    private cubePos nowCube = new cubePos(0, 1, 0);
    [SerializeField] private float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    public GameObject cubeToCreate, AllCubes;
    public GameObject[] canvasStartPage;
    private Rigidbody allCubesRb;
    private bool isToLose, firstCube;
    private List<Vector3> allCubesPOsition = new List<Vector3>
    {
        new Vector3(0, 0, 0), 
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),


    };

    private Coroutine showCubePlace;

    private void Start()
    {
        showCubePlace = StartCoroutine(ShowCubePlace());
        allCubesRb = AllCubes.GetComponent<Rigidbody>();
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }
    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if(Input.GetTouch(0).phase != TouchPhase.Began)
            {
                return;
            }
#endif 
            if (!firstCube)
            {
                firstCube = true;
                foreach(GameObject obj in canvasStartPage)
                {
                    Destroy(obj);
                }
            }



            GameObject newCube = Instantiate(cubeToCreate, cubeToPlace.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(AllCubes.transform);
            nowCube.setVector(cubeToPlace.position);
            allCubesPOsition.Add(nowCube.getVector());
            allCubesRb.isKinematic = true;
            allCubesRb.isKinematic = false;
            SpawnPositions();
        }

        if(!isToLose && allCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(cubeToPlace.gameObject);
            isToLose = true;
            StopCoroutine(showCubePlace);
        }

    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if (isPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x +1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x -1 != cubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y +1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y +1, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y -1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x, nowCube.y -1, nowCube.z));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z +1)) && nowCube.z + 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z +1));
        if (isPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z -1)) && nowCube.z - 1 != cubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z -1));

        cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
    }

    private bool isPositionEmpty(Vector3 targetPos)
    {
        if(targetPos.y == 0)
        {
            return false;
        }
        foreach(Vector3 pos in allCubesPOsition)
        {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
            {
                return true;
            }
        }
        return true;
    }
}
struct cubePos
{
    public int x, y, z;
    public cubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }
    public void setVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}
