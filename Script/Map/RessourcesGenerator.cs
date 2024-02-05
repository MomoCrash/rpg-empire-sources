using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RessourcesGenerator : MonoBehaviour
{

    [SerializeField] [Range(0, 1)] float density;
    [SerializeField] [Range(0, 1)] float randomness;
    [SerializeField] [Range(1, 10000)] public int quantity;

    [SerializeField] GameObject[] GenerableRessources;

    [SerializeField] GameObject layer;

    [SerializeField] float topLeftX;
    [SerializeField] float topLeftY;

    [SerializeField] int maxRightX;
    [SerializeField] int maxDownY;

    public SaveSystem saveSystem;
    public RessourceSave RessourceSave;

    void Start()
    {

        var currentX = topLeftX;
        var currentY = topLeftY;

        if (!saveSystem.IsFirstLoad)
        {

            foreach (var ressource in RessourceSave.GetRessourcesData())
            {

                var newRessources = Instantiate(GenerableRessources[ressource.RessourceIndex], layer.transform);
                newRessources.transform.localPosition = new Vector3(ressource.Position[0], ressource.Position[1], ressource.Position[2]);

            }

            return;
        }

        RessourceSave = new RessourceSave();

        foreach (int value in Enumerable.Range(1, quantity))
        {

            var ressourceIndex = Random.Range(0, GenerableRessources.Length);

            if (Random.Range(0f, 1f) <= randomness)
            {

                var newRessources = Instantiate(GenerableRessources[ressourceIndex], layer.transform);
                newRessources.transform.localPosition = new Vector3(currentX, currentY, 0);

                RessourceSave.AddRessource(newRessources.transform.localPosition, ressourceIndex);

            }

            currentX += Random.Range(1f, 5f * (density + 1));
            currentY -= Random.Range(-3f, 10f);

            if (currentX > maxRightX)
            {
                currentX = topLeftX;
            }
            if (currentY < maxDownY)
            {
                currentY = topLeftY;
            }
        }

    }

    public void RemoveRessourceData(Vector3 position)
    {
        RessourceSave.RemoveRessource(new RessourceData(position, -1));
    }
}

[System.Serializable]
public class RessourceSave
{

    public List<RessourceData> RessourceDatas;

    public RessourceSave()
    {
        RessourceDatas = new List<RessourceData>();
    }

    public void AddRessource(Vector3 position, int id)
    {
        RessourceDatas.Add(new RessourceData(position, id));
    }

    public void RemoveRessource(RessourceData ressource)
    {
        var index = RessourceDatas.FindIndex(re => re.Equals(ressource));
        if (index < 0) return;
        RessourceDatas.RemoveAt(index);
    }

    public RessourceData[] GetRessourcesData()
    {
        return RessourceDatas.ToArray<RessourceData>();
    }

}

[System.Serializable]
public class RessourceData
{

    public float[] Position = new float[3];
    public int RessourceIndex;

    public RessourceData(Vector3 vector, int index)
    {
        this.Position[0] = vector.x;
        this.Position[1] = vector.y;
        this.Position[2] = vector.z;
        this.RessourceIndex = index;
    }

    public override bool Equals(object obj)
    {
        if (obj is not RessourceData)
        {
            return false;
        }

        RessourceData ressource = (RessourceData) obj;

        return (ressource.Position[0] == this.Position[0] 
            && ressource.Position[1] == this.Position[1] 
            && ressource.Position[2] == this.Position[2]);
    }

}
