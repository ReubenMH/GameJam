using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    #region Tree Data
    [System.Serializable]
    public class TreeConfig
    {
        public int StemCount;
        public Vector2Int BranchesPerStem;
        public int LeafCount;
        public float TrunkGrowTime;
        public Vector3 minSize;
        public Vector3 maxSize;
        public Vector3 minRotation;
        public Vector3 maxRotation;
        public GameObject trunkObject;
        public GameObject leafObject;
    }

    public class Trunk
    {
        public Trunk parent;
        public Trunk[] Stems;
        public GameObject trunkObject;
        protected TreeConfig config;
        int trunkCount;

        public Trunk(Trunk parent, TreeConfig config, int trunkCount, Vector3 growthPosition)
        {
            this.parent = parent;
            this.config = config;
            this.trunkCount = trunkCount;

            MakeObject(growthPosition);
            Stems = new Trunk[Random.Range(config.BranchesPerStem.x, config.BranchesPerStem.y)];

            if (trunkCount >= config.StemCount)
            {
                MakeLeaves();
            }
            else
            {
                MakeBranches();
            }
        }

        protected virtual void MakeLeaves()
        {
            for (int i = 0; i < Stems.Length; i++)
            {
                Stems[i] = new Leaf(this, config, trunkCount + 1, Vector3.zero);
            }
        }

        protected virtual void MakeBranches()
        {
            for (int i = 0; i < Stems.Length; i++)
            {
                Stems[i] = new Trunk(this, config, trunkCount + 1, Vector3.zero);
            }
        }

        protected virtual void MakeRootGO()
        {
            trunkObject = Instantiate(config.trunkObject);
        }

        protected virtual void MakeObject(Vector3 growthPosition)
        {
            //Create our object
            MakeRootGO();

            if(parent != null)
                trunkObject.transform.SetParent(parent.trunkObject.transform);

            trunkObject.transform.localPosition = growthPosition;

            Vector3 scale = Vector3.zero;
            scale.x = Random.Range(config.minSize.x, config.maxSize.x);
            scale.y = Random.Range(config.minSize.y, config.maxSize.y);
            scale.z = Random.Range(config.minSize.z, config.maxSize.z);
            trunkObject.transform.localScale = scale;

            Vector3 rotation = Vector3.zero;
            rotation.x = Random.Range(config.minRotation.x, config.maxRotation.x);
            rotation.y = Random.Range(config.minRotation.y, config.maxRotation.y);
            rotation.z = Random.Range(config.minRotation.z, config.maxRotation.z);
            trunkObject.transform.localEulerAngles = rotation;
        }
    }

    public class Leaf : Trunk
    {
        public Leaf(Trunk parent, TreeConfig config, int trunkCount, Vector3 growthPosition) : base(parent, config, trunkCount, growthPosition)
        {
        }

        protected override void MakeBranches()
        {
            //Does nothing
        }

        protected override void MakeLeaves()
        {
            //Does nothing
        }

        protected override void MakeRootGO()
        {
            trunkObject = Instantiate(config.leafObject);
        }
    }

    #endregion

    public static void StartGrowing(TreeConfig config, Vector3 worldPosition)
    {
        GameObject treeObj = new GameObject();
        Tree tree = treeObj.AddComponent<Tree>();
        tree.transform.position = worldPosition;
        tree.StartGrowingInternal(config);
    }

    private void StartGrowingInternal(TreeConfig config)
    {
        Trunk firstTrunk = new Trunk(null, config, 0, transform.position);
    }
}
