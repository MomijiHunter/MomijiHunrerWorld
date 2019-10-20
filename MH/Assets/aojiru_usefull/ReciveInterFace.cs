using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace aojilu
{
    /// <summary>
    ///damage時のメッセージを送る用のinterface
    /// </summary>
    public interface ReciveInterFace_damage : IEventSystemHandler
    {
        /// <summary>
        /// ダメージを食らった時に呼ばれる
        /// </summary>
        void OnReciveDamage();
    }
    /// <summary>
    /// マップ変更時のメッセージを送る用のinterface
    /// </summary>
    public interface ReciveInterFace_mapChenge : IEventSystemHandler
    {
        /// <summary>
        /// マップ変更時に呼ばれる
        /// </summary>
        void OnReciveMapChenge(MapParent mapParent);
    }
}
