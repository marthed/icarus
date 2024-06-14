using UnityEngine;
using UnityEditor.Animations;

public class Recorded : MonoBehaviour
{
    public AnimationClip clip;

    //checkbox to start/stop the recording
    public bool record = false;

    private GameObjectRecorder m_Recorder;

    void Start()
    {
        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    void LateUpdate()
    {
        if (clip == null)
            return;

        if (record)
        {
            //as long as "record" is on: take a snapshot
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }
        else if (m_Recorder.isRecording)
        {
            //"record" is off, but we were recording:
            //save to clip and clear recording
            m_Recorder.SaveToClip(clip);
            m_Recorder.ResetRecording();
        }
    }

    void OnDisable()
    {
        if (clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(clip);
        }
    }
}