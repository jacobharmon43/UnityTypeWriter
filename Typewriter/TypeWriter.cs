using UnityEngine;
#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif
using System.Collections.Generic;

public class TypeWriter : MonoBehaviour{
    private static List<TypeWriterRequestOptions> _currentRequests;
    private static GameObject _typeWriter;

    private static void init(){
        _typeWriter = new GameObject();
        _typeWriter.name = "TypeWriter";
        _typeWriter.AddComponent<TypeWriter>();
        _typeWriter.isStatic = true;
        _currentRequests = new List<TypeWriterRequestOptions>();
        #if !UNITY_EDITOR
            _typeWriter.hideflags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(_typeWriter);
        #else
            if(Application.isPlaying)
                DontDestroyOnLoad(_typeWriter);
        #endif
    }

    private void Update(){
        List<TypeWriterRequestOptions> requestsToRemove = new List<TypeWriterRequestOptions>();
        foreach(var request in _currentRequests){

            #if ENABLE_INPUT_SYSTEM
                Debug.Log("New");
                if(Keyboard.current[(Key)request.AutoFinishTypingButton].wasPressedThisFrame){
                    request.TMPTextObject.text = request.FullString;
                    request.CompletionEvent?.Invoke();
                    requestsToRemove.Add(request);
                    continue;
                }
            #else
                Debug.Log("Old");
                if(Input.GetKeyDown(request.AutoFinishTypingButton)){
                    request.TMPTextObject.text = request.FullString;
                    request.CompletionEvent?.Invoke();
                    requestsToRemove.Add(request);
                    continue;
                }
            #endif

            request.currentTimer += Time.deltaTime;
            float charCount = request.FullString.Length;
            float currCharCount = request.currentString.Length;

            if(request.currentTimer/request.TimeToCompletion >= (currCharCount + 1)/charCount){
                request.currentString += request.FullString[(int)currCharCount];
                request.TMPTextObject.text = request.currentString;
                request.LetterPlaceEvent?.Invoke();
            }


            if(request.currentTimer >= request.TimeToCompletion){
                request.TMPTextObject.text = request.FullString;
                request.CompletionEvent?.Invoke();
                requestsToRemove.Add(request);
            }
        }

        foreach(var r in requestsToRemove){
            _currentRequests.Remove(r);
        }
    }

    public static void CompleteRequest(TypeWriterRequestOptions request){
        request.TMPTextObject.text = request.FullString;
        request.CompletionEvent?.Invoke();
        _currentRequests.Remove(request);
    }

    public static void AddRequest(TypeWriterRequestOptions request){
        if(!_typeWriter)
            init();
        _currentRequests.Add(request);
    }
}
