using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{
    [CreateAssetMenu(menuName = "R2/Items/Cloth Item")]

    public class ClothItem : ScriptableObject
    {
        public ClothItemType clothType;
        public Mesh mesh;
        public Material clothMaterial;

    }

}