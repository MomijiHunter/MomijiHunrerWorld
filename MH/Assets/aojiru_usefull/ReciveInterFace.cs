using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace aojilu
{
    public interface ReciveInterFace_damage : IEventSystemHandler
    {
        /// <summary>
        /// ダメージを食らった時に呼ばれる
        /// </summary>
        void OnReciveDamage();
    }
    public interface ReciveInterFace_mapChenge : IEventSystemHandler
    {
        /// <summary>
        /// ダメージを食らった時に呼ばれる
        /// </summary>
        void OnReciveMapChenge(MapParent mapParent);
    }
}
