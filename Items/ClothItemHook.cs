using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{

    public class ClothItemHook : MonoBehaviour
    {
        public ClothItemType clothItemType;   
        public SkinnedMeshRenderer meshRenderer;
        public Mesh defaultMesh;   
        public Material defaultMaterial;  

        private void Awake()
        {
            // find cloth manager in parent
            ClothManager clothManager = GetComponentInParent<ClothManager>();
            clothManager.RegisterClothHook(this);


        }

        internal void LoadClothItem(ClothItem clothItem)
        {
            meshRenderer.sharedMesh = clothItem.mesh;
            meshRenderer.material = clothItem.clothMaterial;
        }

        internal void UnloadItem()
        {
            // we don't really need a reference to the material because we have our item type
            if (clothItemType.isDisabledWhenNoItem)
            {
                meshRenderer.enabled = false;
            }
            else
            {
                meshRenderer.sharedMesh = defaultMesh;
                meshRenderer.material = defaultMaterial;
            }
        }
    }
}
