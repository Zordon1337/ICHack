using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;

namespace HD
{
    class Main:MonoBehaviour
    {
        PlayerRaycast LocalPlayer;
        Thief[] Thiefs;
        Civil[] Civils;

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        static System.Random Rand = new System.Random();
        
        IEnumerator SpinBotCivil()
        {
            for(; ; )
            {
                if (config.CivilDoesHvH)
                {
                    foreach (var civil in Civils)
                    {
                        civil.transform.eulerAngles = new Vector3(Rand.Next(0, 180), Rand.Next(0, 180), Rand.Next(0, 180));
                        

                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        IEnumerator CollectCivils()
        {
            for(; ; )
            {
                Civils = FindObjectsOfType<Civil>();

                yield return new WaitForSeconds(0.1f);
            }
        }
        IEnumerator CollectThiefs()
        {
            for (; ; )
            {
                Thiefs = FindObjectsOfType<Thief>();
                yield return new WaitForSeconds(0.1f);
            }
        }
        IEnumerator CollectLocal()
        {
            for (; ; )
            {
                LocalPlayer = FindObjectOfType<PlayerRaycast>();
                yield return new WaitForSeconds(0.1f);
            }
        }
        IEnumerator CheckMenuKey()
        {
            for(; ; )
            {
                if (Input.GetKey(KeyCode.Insert))
                {

                    config.MenuOpen = !config.MenuOpen;
                }
                // we don't want to have spamming menu(closing and opening at same time)
                yield return new WaitForSeconds(0.15f);
            }
        }
        IEnumerator RandomColor()
        {
            for(; ; )
            {
                GUI.backgroundColor = new Color(1f,0f,0f, 1f);
                GUI.contentColor = new Color(0f, 0f, 1f, 1f);
                yield return new WaitForSeconds(0.1f);
            }
                
        }
        void Start()
        {
            StartCoroutine(CollectLocal());
            StartCoroutine(CollectThiefs());
            StartCoroutine(CollectCivils());
            StartCoroutine(SpinBotCivil());
            StartCoroutine(CheckMenuKey());
            StartCoroutine(RandomColor());
        }
        void Update() 
        {
            if(config.AimbotThief)
            {
                foreach(var Thief in Thiefs)
                {
                    Vector3 World2Screen = Camera.main.WorldToScreenPoint(Thief.transform.position);
                    
                    if(World2Screen != Vector3.zero)
                    {
                        Vector2 Target = new Vector2(World2Screen.x, Screen.height - World2Screen.y);
                        // if distance is less than 300 do aimbot
                        if (Vector3.Distance(Camera.main.transform.position, Thief.transform.position) <= 10 )
                        {
                            double AimX = Target.x - Screen.width / 2.0f;
                            double AimY = Target.y - Screen.height / 2.0f-420f; // 420 isn't random number :troll_face:
                            mouse_event(0x0001, (int)AimX, (int)AimY, 0, 0);
                        }
                        
                    }
                }
            }
        }
        void RenderGUI(int windowID)
        {
            GUI.backgroundColor = new Color(0.035f, 0.89f, 1, 1f);
            GUI.color = Color.white;
            if (GUILayout.Button("Delete civils"))
            {
                foreach (var Civil in Civils)
                {
                    Civil.transform.gameObject.SetActive(false);
                }
            }
            if (GUILayout.Button("Delete thief"))
            {
                foreach (var Thief in Thiefs)
                {
                    Thief.transform.gameObject.SetActive(false);
                }
            }
            if (GUILayout.Button("Civil lines ESP: " + config.CivilLines))
            {
                config.CivilLines = !config.CivilLines;
            }
            if (GUILayout.Button("Civil name ESP: " + config.CivilName))
            {
                config.CivilName = !config.CivilName;
            }
            if (GUILayout.Button("Thief Lines ESP: " + config.ThiefLines))
            {
                config.ThiefLines = !config.ThiefLines;
            }
            if (GUILayout.Button("Thief name ESP: " + config.ThiefName))
            {
                config.ThiefName = !config.ThiefName;
            }
            if (GUILayout.Button("Civil does hvh(spinbot): " + config.CivilDoesHvH))
            {
                config.CivilDoesHvH = !config.CivilDoesHvH;
            }
            if (GUILayout.Button("Aimbot Thief: "+config.AimbotThief))
            {
                config.AimbotThief = !config.AimbotThief;
            }
            if (GUILayout.Button("Add 1 milion noney"))
            {
                PlayerPrefs.SetFloat("money", 1000000);
            }
            
            GUILayout.Label("ESP Render Distance");
            config.distanceesp = GUILayout.HorizontalSlider(config.distanceesp, 1, 100);
        }
        void OnGUI()
        {
            foreach (var civil in Civils)
            {
                var w2s = Camera.main.WorldToScreenPoint(civil.transform.position);
                
                if(config.CivilLines)
                {
                    if(Vector3.Distance(Camera.main.transform.position,civil.transform.position) <= config.distanceesp)
                    {
                        Render.DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(w2s.x, Screen.height - w2s.y-2), Color.red, 2f);
                    }
                    
                }
                if (config.CivilName)
                {
                    if (Vector3.Distance(Camera.main.transform.position, civil.transform.position) <= config.distanceesp)
                    {
                        GUI.Label(new Rect(w2s.x, (float)Screen.height - w2s.y, 200, 20), "Civil");
                    }
                }
                
            }
            foreach (var Thief in Thiefs)
            {
                var w2s = Camera.main.WorldToScreenPoint(Thief.transform.position);
                 
                if (config.ThiefLines)
                {
                    if (Vector3.Distance(Camera.main.transform.position, Thief.transform.position) <= config.distanceesp)
                    {
                        Render.DrawLine(new Vector2(Screen.width / 2, Screen.height), new Vector2(w2s.x, Screen.height - w2s.y-2), Color.red, 2f);
                    }
                }
                if (config.ThiefName)
                {
                    if (Vector3.Distance(Camera.main.transform.position, Thief.transform.position) <= config.distanceesp)
                    {
                        GUI.Label(new Rect(w2s.x, (float)Screen.height - w2s.y, 200, 20), "Thief");
                    }
                }
                
            }
            GUI.backgroundColor = new Color(0.42f, 0.098f, 0.749f, 1f);
            GUI.contentColor = new Color(0f, 1f, 0f, 1f);
            if (config.MenuOpen)
            {
                GUI.Window(0, new Rect(Screen.width/2-300, Screen.height/2-225, 600, 450), RenderGUI, "ICHack");
            }
            
            

        }
    }
}
