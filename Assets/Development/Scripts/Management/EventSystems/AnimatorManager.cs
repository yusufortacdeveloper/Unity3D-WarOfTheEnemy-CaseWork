using UnityEngine;

public class AnimatorManager : MonoBehaviour,IEventListener
{
    private void OnEnable()
    {
        EventManager.Instance?.RegisterListener(this);
    }
    private void OnDisable()
    {
        EventManager.Instance?.UnregisterListener(this);
    }
    public void PlayAnimation(Animator animatorController, string animationName,bool justThisAnimPlay)
    {
        if (animatorController == null)
        {
            Debug.LogError("AnimatorController is null!");
            return;
        }

        if(justThisAnimPlay)
        {
            foreach (AnimatorControllerParameter parameter in animatorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    animatorController.SetBool(parameter.name, false);
                }
            }
            animatorController.SetBool(animationName, true);
        }
        else
        {
            animatorController.SetBool(animationName, true);
        }
    }
}
