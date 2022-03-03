using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tree : MonoBehaviour
{
    #region Tree Data
    [System.Serializable]
    public class TreeConfig
    {
        public int StemCount;
        public Vector2Int BranchesPerStem;
        public Vector2Int LeafCount;
        public Vector2 TrunkGrowTime;
        public Vector3 minTrunkSize;
        public Vector3 maxTrunkSize;
        public Vector3 minLeafSize;
        public Vector3 maxLeafSize;
        public Vector3 minRotation;
        public Vector3 maxRotation;
        public GameObject trunkObject;
        public GameObject leafObject;
        public float trunkStraightness;
    }

    public class Trunk
    {
        public Trunk parent;
        public Trunk[] Stems;
        public GameObject trunkObject;
        public TrunkElement trunkElement;
        protected TreeConfig config;
        int trunkCount;
        protected Vector3 treeScale;

        public Trunk(Trunk parent, TreeConfig config, int trunkCount, Vector3 growthPosition, Vector3 growthDirection)
        {
            this.parent = parent;
            this.config = config;
            this.trunkCount = trunkCount;

            MakeObject(growthPosition, growthDirection);
            MakeChildrenCount();

            if (trunkCount >= config.StemCount)
            {
                MakeLeaves();
            }
            else
            {
                MakeBranches();
            }
        }

        protected virtual void MakeChildrenCount()
        {
            Stems = new Trunk[Random.Range(config.BranchesPerStem.x, config.BranchesPerStem.y)];
        }

        protected virtual void MakeLeaves()
        {
            Vector3 anchorPosition = trunkObject.transform.localScale / 2f;
            for (int i = 0; i < Stems.Length; i++)
            {
                Stems[i] = new Leaf(this, config, trunkCount + 1, anchorPosition, Vector3.zero);
            }
        }

        protected virtual void MakeBranches()
        {
            Vector3 anchorPosition = trunkObject.transform.localScale / 2f;
            for (int i = 0; i < Stems.Length; i++)
            {
                Stems[i] = new Trunk(this, config, trunkCount + 1, anchorPosition, Vector3.zero);
            }
        }

        protected virtual void MakeRootGO()
        {
            trunkObject = Instantiate(config.trunkObject);
        }

        protected virtual void MakeObject(Vector3 growthPosition, Vector3 growthDirection)
        {
            //Create our object
            MakeRootGO();

            if(parent != null)
                trunkObject.transform.SetParent(parent.trunkObject.GetComponent<TrunkElement>().offsetTrunk);

            trunkObject.transform.localPosition = growthPosition;

            
            
            treeScale = GetSize();
            trunkObject.transform.localScale = Vector3.zero * 0.001f;
            trunkElement = trunkObject.GetComponent<TrunkElement>();
            trunkElement.SetActive(false);

            Vector3 rotation = Vector3.zero;
            rotation.x = Random.Range(config.minRotation.x, config.maxRotation.x);
            rotation.y = Random.Range(config.minRotation.y, config.maxRotation.y);
            rotation.z = Random.Range(config.minRotation.z, config.maxRotation.z);
            rotation *= trunkCount == 0 ? config.trunkStraightness : 1;
            trunkObject.transform.localEulerAngles = rotation;
        }

        protected virtual Vector3 GetSize()
        {
            Vector3 scale = Vector3.zero;
            scale.x = Random.Range(config.minTrunkSize.x, config.maxTrunkSize.x);
            scale.y = Random.Range(config.minTrunkSize.y, config.maxTrunkSize.y);
            scale.z = Random.Range(config.minTrunkSize.z, config.maxTrunkSize.z);
            return scale / 1.5f;
        }

        public virtual void Grow()
        {
            trunkElement.SetActive(true);
            trunkObject.transform.DOScale(treeScale, Random.Range(config.TrunkGrowTime.x, config.TrunkGrowTime.y)).OnComplete(() =>
            {
                for(int i = 0; i < Stems.Length; i++)
                {
                    Stems[i].Grow();
                }
            });
        }
    }

    public class Leaf : Trunk
    {
        public Leaf(Trunk parent, TreeConfig config, int trunkCount, Vector3 growthPosition, Vector3 growthDirection) : base(parent, config, trunkCount, growthPosition, growthDirection)
        {
        }

        protected override void MakeChildrenCount()
        {
            Stems = new Trunk[Random.Range(config.LeafCount.x, config.LeafCount.y)];
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

        protected override Vector3 GetSize()
        {
            Vector3 scale = Vector3.zero;
            scale.x = Random.Range(config.minLeafSize.x, config.maxLeafSize.x);
            scale.y = Random.Range(config.minLeafSize.y, config.maxLeafSize.y);
            scale.z = Random.Range(config.minLeafSize.z, config.maxLeafSize.z);
            return scale;
        }
    }

    #endregion

    public static void StartGrowing(Transform parent, TreeConfig config, Vector3 worldPosition, Vector3 startDirection)
    {
        Trunk firstTrunk = new Trunk(null, config, 0, worldPosition, startDirection);

        if(parent != null)
            firstTrunk.trunkObject.transform.SetParent(parent, true);
        
        
        
        firstTrunk.Grow();
    }
}
