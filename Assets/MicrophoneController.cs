using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using HuggingFace.API;
using Unity.Netcode;
using System.Text.RegularExpressions;


public class MicrophoneController : NetworkBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private string correctLyrics;
    [SerializeField] private GameManager gameManager;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    void Start() {
        correctLyrics = Regex.Replace(correctLyrics, "[^0-9a-zA-Z]+", "");
    }

    private void Update()
    {
        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
    }

    public void StartRecording()
    {
        Debug.Log("Recording started");
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    public void StopRecording()
    {
        Debug.Log("Recording stopped");
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    private void SendRecording()
    {
        text.color = Color.yellow;
        text.text = "Sending...";
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {

            string originalResponse = response;
            Debug.Log(originalResponse);
            string cleanedResponse = Regex.Replace(response, "[^0-9a-zA-Z]+", "");


            if (string.Equals(Regex.Replace(cleanedResponse, @"\s+", ""), correctLyrics, System.StringComparison.InvariantCultureIgnoreCase)) {
                DisplayResultClientRpc("¡Éxito!", Color.green);
                SucceedClientRpc();

            } else
            {
                DisplayResultClientRpc(originalResponse, Color.yellow);
            }
        }, error => {
            DisplayResultClientRpc("Error...", Color.red);
        });
    }

    [ClientRpc]
    public void DisplayResultClientRpc(string result, Color color)
    {
        text.text = result;
        text.color = color;
    }

    [ClientRpc]
    public void SucceedClientRpc() {
        gameManager.OnRoomSolved();
        this.gameObject.SetActive(false);
    }
}
