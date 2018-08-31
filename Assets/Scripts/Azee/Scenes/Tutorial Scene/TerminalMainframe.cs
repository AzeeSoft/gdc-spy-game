using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TerminalMainframe : MonoBehaviour
{
    [Serializable]
    public class ServerGroup
    {
        public List<Server> Servers;

        public void Infect()
        {
            foreach (Server server in Servers)
            {
                server.Infect();
            }
        }
    }

    public MeshRenderer CapsuleMeshRenderer;
    public TextMeshPro Text;

    public List<ServerGroup> BackRoomServerGroups;
    public List<ServerGroup> FinalRoomServerGroup;

    public GameObject FinalRoomLights;

    private InteractiveObject _interactiveObject;
    private AudioController _audioController;

    private bool _malwareInjected = false;
    private IEnumerator _serverInfectionCoroutine;

    void Awake()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _audioController = GetComponent<AudioController>();
    }

    void Update()
    {

    }

    void OnEnable()
    {
        if (_serverInfectionCoroutine != null)
        {
            StartCoroutine(_serverInfectionCoroutine);
        }
    }

    void OnDisable()
    {

    }

    public void InjectMalware()
    {
        if (!_malwareInjected)
        {
            _malwareInjected = true;
            _interactiveObject.enabled = false;

            TutorialManager.Instance.BroadcastTutorialAction("malwareInjectionStarted");

            _serverInfectionCoroutine = InfectServers();
            StartCoroutine(_serverInfectionCoroutine);
        }
    }

    IEnumerator InfectServers()
    {
        _audioController.PlayClip(2);

        foreach (Guard guard in FindObjectsOfType<Guard>())
        {
            guard.enabled = false;
        }

        yield return new WaitForSeconds(6f);

        yield return InfectBackRoomServers();

        yield return new WaitForSeconds(2f);

        yield return InfectFinalRoomServers();

        if (FinalRoomLights)
        {
            FinalRoomLights.SetActive(false);
        }

        CapsuleMeshRenderer.material.color = Color.red;
        CapsuleMeshRenderer.material.SetColor("_RimColor", Color.red);

        Text.text = "System Down";
        Text.color = Color.red;

        TutorialManager.Instance.BroadcastTutorialAction("malwareInjected");

        _serverInfectionCoroutine = null;
    }

    IEnumerator InfectBackRoomServers()
    {
        foreach (ServerGroup serverGroup in BackRoomServerGroups)
        {
            serverGroup.Infect();

            _audioController.PlayClip(3);

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator InfectFinalRoomServers()
    {
        foreach (ServerGroup serverGroup in FinalRoomServerGroup)
        {
            serverGroup.Infect();
        }

        _audioController.PlayClip(4);

        yield return new WaitForSeconds(0f);
    }
}