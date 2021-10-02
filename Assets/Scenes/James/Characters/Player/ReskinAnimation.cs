using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReskinAnimation : MonoBehaviour
{
    public string spriteSheetName;

    private void LateUpdate()
    {
        var subsprites = Resources.LoadAll<Sprite>("Characters/" + spriteSheetName);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteName = renderer.sprite.name;
            int pos = spriteName.LastIndexOf("_") + 1;
            string suffix = spriteName.Substring(pos, spriteName.Length - pos);
            var newSprite = Array.Find(subsprites, item => item.name.EndsWith(suffix));

            if (newSprite)
            {
                renderer.sprite = newSprite;
            }
        }
    }
}
