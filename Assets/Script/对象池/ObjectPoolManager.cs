using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _particleSystemsEmpty;
    private static GameObject _gameObjectEmpty;

    public enum PoolType
    {
       GameObject,
       ParticleSystem,
        None
    }

    public static PoolType poolType = PoolType.None;

    private void Awake()
    {
        SetupEmpties();
    }


    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemsEmpty = new GameObject("Particle Effect");
        _particleSystemsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectEmpty = new GameObject("GameObjects");
        _gameObjectEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

    }   

    public static GameObject SpawnObject(GameObject objectToSpawn,Vector3 spawnPosition,Quaternion spawnRotation,PoolType poolType)
    {
        PooledObjectInfo pool=objectPools.Find(p=>p.LookupString==objectToSpawn.name);

        if(pool==null) 
        {
            pool = new PooledObjectInfo() { LookupString=objectToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObj=pool.InactiveObjects.FirstOrDefault();
        //GameObject spawnableObj = null;
        //foreach(GameObject Obj in pool.InactiveObjects)
        //{
        //    if (Obj != null)
        //    {
        //        spawnableObj = Obj;
        //        break;
        //    }
        //}

        if(spawnableObj==null) 
        {
            GameObject parentObject = SetParentObject(poolType);

            spawnableObj = Instantiate(objectToSpawn,spawnPosition,spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);//by taking off 7,we are removeing the (Clone) from the name
        PooledObjectInfo pool=objectPools.Find(p=> p.LookupString==goName);

        if(pool==null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled +" + obj.name);
        }

        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }


    private static GameObject SetParentObject(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemsEmpty;

            case PoolType.GameObject:
                return _gameObjectEmpty;

            case PoolType.None:
                return null;

            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects=new List<GameObject>();
}