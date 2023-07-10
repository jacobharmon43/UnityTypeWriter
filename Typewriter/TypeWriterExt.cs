using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// ONLY SUPPORTS TMPro_Text objects
/// Defaults for class are: {TimeToCompletion: 5f, CompletionEvent null, LetterPlaceEvent null, AutoFinishTypingButton none }
/// </summary>
public class TypeWriterRequestOptions{
    public string FullString;
    public TMP_Text TMPTextObject;
    public float TimeToCompletion;
    public UnityEvent CompletionEvent;
    public UnityEvent LetterPlaceEvent;
    public KeyCode AutoFinishTypingButton;

    public float currentTimer = 0;
    public string currentString = "";
}

public static class TypeWriterExt{
    public static TypeWriterRequestOptions CreateTypeWriterRequest(this TMP_Text text, string desiredText){
        return new TypeWriterRequestOptions(){
            FullString = desiredText,
            TMPTextObject = text,
            TimeToCompletion = 5,
            CompletionEvent = new UnityEvent(),
            LetterPlaceEvent = new UnityEvent(),
            AutoFinishTypingButton = KeyCode.None
        };
    }

    public static TypeWriterRequestOptions SetCompletionTime(this TypeWriterRequestOptions options, float time){
        options.TimeToCompletion = time;
        return options;
    }

    public static TypeWriterRequestOptions AddCompletionEvent(this TypeWriterRequestOptions options, UnityAction completionEvent){
        options.CompletionEvent.AddListener(completionEvent);
        return options;
    }

    public static TypeWriterRequestOptions AddLetterPlaceEvent(this TypeWriterRequestOptions options, UnityAction letterPlaceEvent){
        options.LetterPlaceEvent.AddListener(letterPlaceEvent);
        return options;
    }

    public static TypeWriterRequestOptions SetAutoCompletionKey(this TypeWriterRequestOptions options, KeyCode skipKey){
        options.AutoFinishTypingButton = skipKey;
        return options;
    }

    public static void SendRequest(this TypeWriterRequestOptions options){
        options.TMPTextObject.text = options.currentString;
        TypeWriter.AddRequest(options);
    }
}
