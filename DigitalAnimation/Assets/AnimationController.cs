using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    public string currentCreator;
    [SerializeField] public Dictionary<string, List<AnimationObject>> animationMap;

    public Text creatorText;
    public GameObject arrowPanel;

    GameObject currentAnim;
    int currentIndex = 0;
    int currentLength = 0;

    public void Awake()
    {
        animationMap = new Dictionary<string, List<AnimationObject>>();
        foreach (GameObject g in Resources.LoadAll<GameObject>("Animations"))
        {
            string s = g.GetComponent<AnimationObject>().creator.ToLower();
            animationMap[s] = GetAnimationList((animationMap.ContainsKey(s) ? animationMap[s] : null), Instantiate<GameObject>(g).GetComponent<AnimationObject>());
            currentLength = animationMap[s].Count;
            for (int i = 0; i < currentLength; i++)
                animationMap[s][i].gameObject.SetActive(false);
        }
        currentLength = 0;
    }

    void Update()
    {
        if (currentLength <= 1)
        {
            arrowPanel.SetActive(false);
        }
        else
        {
            arrowPanel.SetActive(true);
        }

    }
    public List<AnimationObject> GetAnimationList(List<AnimationObject> currentList, AnimationObject toAdd)
    {
        List<AnimationObject> list = (currentList == null ? new List<AnimationObject>() : new List<AnimationObject>(currentList));
        list.Add(toAdd);
        Debug.Log(toAdd.name + " " +list.Count);
        return list;
    }

    public void DisplayAnimationByCreator(InputField creator)
    {
        if (currentAnim != null)
        {
            currentAnim.SetActive(false);
            currentAnim = null;
            currentIndex = 0;
        }

        Debug.Log("Displaying ...");
        if (animationMap.ContainsKey(creator.text.ToLower()))
        {
            currentIndex = 0;
            currentLength = animationMap[creator.text.ToLower()].Count;
            GameObject g = animationMap[creator.text.ToLower()][currentIndex].gameObject;
            g.SetActive(true);
            Debug.Log("Setting active " + g.name);

            currentAnim = g;
            creatorText.text = currentAnim.GetComponent<AnimationObject>().creator;
        }

        creator.text = "";
    }

    public void SwitchAnimation_SameCreator(bool right)
    {
        currentLength = animationMap[creatorText.text.ToLower()].Count;

        if (right)
        {
            currentIndex++;
            if (currentIndex >= currentLength) currentIndex = 0;
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = currentLength - 1;
        }
        Debug.Log("Switching animation " + currentIndex);

        currentAnim.gameObject.SetActive(false);
        currentAnim = animationMap[creatorText.text.ToLower()][currentIndex].gameObject;
        currentAnim.gameObject.SetActive(true);
    }
}
