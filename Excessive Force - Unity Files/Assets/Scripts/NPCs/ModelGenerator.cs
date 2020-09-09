using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGenerator : MonoBehaviour
{
    [Header("Existing Components")]
    [SerializeField] private List<ModelFactory> factories;
    [SerializeField] private SkeletonBoneController theSkeleton;
    [SerializeField] private Transform spawnParent;

    [Header("Generation Options")]
    public List<ModelPartTypes> partsToSpawn;
    public List<Material> colorPallette;

    // Start is called before the first frame update
    void Start()
    {
        GenerateModel();
        ColorModel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateModel()
    {
        foreach (ModelPartTypes mpt in partsToSpawn)
        {
            // Getting relevant Factory
            ModelFactory factory = null;
            foreach (ModelFactory mf in factories)
            {
                if (mf.factoryType == mpt)
                {
                    factory = mf;
                    break;
                }
            }

            // Handling New Model Part
            GameObject newPart = factory.GetGameObject();
            SkeletonBoneAssigner[] boneAssigners = newPart.GetComponentsInChildren<SkeletonBoneAssigner>();
            newPart.transform.parent = spawnParent;

            // Setting Skinned Mesh Renderer To Follow New Skeleton
            foreach (SkeletonBoneAssigner sba in boneAssigners)
            {
                List<Transform> newBones = new List<Transform>();

                foreach (BoneType bt in sba.boneTypes)
                {
                    newBones.Add(theSkeleton.GetBone(bt).transform);
                }

                sba.gameObject.GetComponent<SkinnedMeshRenderer>().bones = newBones.ToArray();
            }

        }
    }

    private void ColorModel()
    {
        Renderer[] modelMeshes = this.GetComponentsInChildren<Renderer>();

        foreach (Renderer modelRenderers in modelMeshes)
        {
            List<Material> newMaterials = new List<Material>();

            foreach (Material m in modelRenderers.sharedMaterials)
            {
                Material materialToAdd = null;

                for (int i = 0; i < colorPallette.Count; i++)
                {
                    if (m.name == colorPallette[i].name)
                    {
                        materialToAdd = colorPallette[i];
                        break;
                    }
                }

                if (materialToAdd == null)
                {
                    materialToAdd = m;
                }

                newMaterials.Add(materialToAdd);
            }

            modelRenderers.sharedMaterials = newMaterials.ToArray();
        }
    }
}
