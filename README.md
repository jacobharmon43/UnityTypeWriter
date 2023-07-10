# UnityTypeWriter
Quick typewriter library for easy setup and use.

Example implementation class below:

```cs
using UnityEngine;

public class TestWriter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] AudioClip[] _clips;

    private void Start(){
        text.CreateTypeWriterRequest("Large block of text to be created")
            .SetCompletionTime(3)
            .AddCompletionEvent(() => Debug.Log("DONE"))
            .AddLetterPlaceEvent(() => AudioSource.PlayClipAtPoint(_clips[Random.Range(0, _clips.Length)], Vector3.zero))
            .SetAutoCompletionKey(KeyCode.Space)
            .SendRequest();
    }
}
```
