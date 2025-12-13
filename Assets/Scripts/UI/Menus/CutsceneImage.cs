using UnityEngine;

namespace DefaultNamespace
{

    [CreateAssetMenu(menuName = "Cutscenes/Cutscene Images")]
    public class CutsceneImage : ScriptableObject
    {
        public int index;
        public Sprite sprite;
    }
}