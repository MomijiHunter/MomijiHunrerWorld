using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aojilu
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] GameObject spritePrefab;
        [SerializeField] GameObject layerParentPrafab;
        [SerializeField] Vector2 spriteSize;
        [SerializeField] string mapChipName;

        [SerializeField] string[] mapDataList;
        [SerializeField] bool[] mapHitList;
        [SerializeField] string[] mapTagList;

        /// <summary>
        /// マップの生成
        /// </summary>
        public void CreatMap()
        {

            Sprite[] sprites = Resources.LoadAll<Sprite>(mapChipName);
            List<TextAsset> mapCSVList = new List<TextAsset>();
            foreach (var s in mapDataList)
            {
                mapCSVList.Add(Resources.Load<TextAsset>(s));
            }

            #region sprite生成処理
            for (int z = 0; z < mapCSVList.Count; z++)
            {
                GameObject layerParent = Instantiate(layerParentPrafab, Vector2.zero, Quaternion.identity);
                layerParent.name = "Layer_" + z.ToString();
                layerParent.transform.SetParent(transform);

                string[] lines = mapCSVList[z].text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int y = 0; y < lines.Length; y++)
                {
                    string[] words = lines[y].Split(new char[] { ',' }, StringSplitOptions.None);
                    for (int x = 0; x < words.Length; x++)
                    {
                        int value;
                        if (!int.TryParse(words[x].Trim(), out value)) continue;
                        if (value < 0) continue;
                        if (value >= sprites.Length) continue;

                        var obj = GameObject.Instantiate(this.spritePrefab);
                        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = sprites[value];
                        spriteRenderer.sortingOrder = z;
                        obj.transform.SetParent(layerParent.transform);
                        obj.transform.localPosition = new Vector3(x * this.spriteSize.x, -y * this.spriteSize.y, 0);

                        //当たり判定
                        if (mapHitList[z])
                        {
                            BoxCollider2D box = obj.AddComponent<BoxCollider2D>() as BoxCollider2D;
                            box.size = new Vector2(1,1);
                        }
                        foreach(var s in mapTagList)
                        {
                            if (s != null && s != "") obj.tag = s;
                        }
                        
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// マップの破壊
        /// </summary>
        public void DestroyMap()
        {
            List<GameObject> removes_parent = new List<GameObject>();
            List<GameObject> removes_sprite = new List<GameObject>();
            var transform = this.GetComponent<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {

                var trans = transform.GetChild(i);
                if (trans == null) continue;

                removes_parent.Add(trans.gameObject);

                for (int j = 0; j < trans.childCount; j++)
                {
                    var trans2 = trans.transform.GetChild(j);
                    if (trans2 == null) continue;
                    removes_sprite.Add(trans2.gameObject);
                }
            }
#if UNITY_EDITOR
            removes_sprite.ForEach(x => DestroyImmediate(x));
            removes_parent.ForEach(x => DestroyImmediate(x));
#else
            removes_sprite.ForEach(x => Destroy(x));
            removes_parent.ForEach(x => Destroy(x));
#endif
        }
    }
}
