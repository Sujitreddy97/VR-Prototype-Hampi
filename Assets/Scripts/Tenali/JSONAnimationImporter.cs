using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi
{
    public class JSONAnimationImporter : MonoBehaviour
    {
        [System.Serializable]
        public class KeyframeData
        {
            public float time;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        [System.Serializable]
        public class AnimationData
        {
            public List<KeyframeData> animation;
        }

        public TextAsset jsonFile; // Direct reference to the JSON file
        public GameObject targetObject; // The object to animate

        private AnimationData animationData;
        private bool isPlaying = false;

        void Start()
        {
            LoadJSON();
            StartCoroutine(PlayAnimation());
        }

        void LoadJSON()
        {
            if (jsonFile == null)
            {
                Debug.LogError("JSON file is not assigned in the Inspector.");
                return;
            }

            animationData = JsonUtility.FromJson<AnimationData>(jsonFile.text);
        }

        IEnumerator PlayAnimation()
        {
            if (animationData == null || animationData.animation.Count == 0)
            {
                Debug.LogError("No animation data found.");
                yield break;
            }

            isPlaying = true;
            float startTime = Time.time;

            for (int i = 0; i < animationData.animation.Count - 1; i++)
            {
                KeyframeData currentFrame = animationData.animation[i];
                KeyframeData nextFrame = animationData.animation[i + 1];

                float frameDuration = nextFrame.time - currentFrame.time;
                float elapsedTime = 0;

                while (elapsedTime < frameDuration)
                {
                    if (!isPlaying)
                        yield break;

                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / frameDuration;

                    // Interpolate position, rotation, and scale
                    targetObject.transform.position = Vector3.Lerp(
                        currentFrame.position,
                        nextFrame.position,
                        t
                    );

                    targetObject.transform.rotation = Quaternion.Lerp(
                        Quaternion.Euler(currentFrame.rotation),
                        Quaternion.Euler(nextFrame.rotation),
                        t
                    );

                    targetObject.transform.localScale = Vector3.Lerp(
                        currentFrame.scale,
                        nextFrame.scale,
                        t
                    );

                    yield return null;
                }
            }

            isPlaying = false;
        }

        public void StopAnimation()
        {
            isPlaying = false;
            StopAllCoroutines();
        }
    }
}
