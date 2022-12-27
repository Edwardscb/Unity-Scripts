using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R2
{

    public class ClothManager : MonoBehaviour
    {
        // using dictionary instead of list because it might be faster
        // keep in mind you can only wear one shirt
        Dictionary<ClothItemType, ClothItemHook> clothHooks = new Dictionary<ClothItemType, ClothItemHook>();

        public void Init()
        {

        }

        public void RegisterClothHook(ClothItemHook clothItemHook)
        {
            // first check if something is there, if not, add to dictionary
            if (!clothHooks.ContainsKey(clothItemHook.clothItemType))
            {
                clothHooks.Add(clothItemHook.clothItemType, clothItemHook);
            }
        }

        ClothItemHook getClothHook(ClothItemType target)
        {
            clothHooks.TryGetValue(target, out ClothItemHook retVal);
            // this returns the cloth hook which means, when we are passing an actual cloth item, on the cloth item we are also going to have a cloth item type
                return retVal;
                
        }

        public void LoadItem(ClothItem clothItem, ClothItemType clothItemType)
        {
            ClothItemHook itemHook = null;

            if(clothItem == null)
            {
                if (clothItemType != null)
                {
                    itemHook = getClothHook(clothItemType);
                    if (clothItemType.isDisabledWhenNoItem)
                    {
                        itemHook.UnloadItem();
                        // he did it again here (quick action generate)
                    }

                    return;
                }

            }
            
            itemHook = getClothHook(clothItemType);
            itemHook.LoadClothItem(clothItem);
            // he used a quick action to generate the LoadClothItem method onto ClothItemHook - be sure to get that method


        }
    }
}
