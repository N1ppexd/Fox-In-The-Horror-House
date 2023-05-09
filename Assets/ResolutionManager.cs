using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq.Expressions;

namespace HorrorFox
{
    public class ResolutionManager : MonoBehaviour
    {

        [SerializeField] private TMP_Dropdown resolutionDropdown, aspectRatioDropdown;

        string currentScreenAspect;


        void Start()
        {
            string currentAspectRatio = GetCurrentAspectRatio();
            Debug.Log("Current aspect ratio: " + currentAspectRatio);

            SetAspectRatio(currentAspectRatio);

            switch (currentAspectRatio)
            {
                case "16:9":
                    aspectRatioDropdown.value = 0;
                    return;
                case "16:10":
                    aspectRatioDropdown.value = 1;
                    return;
                case "4:3":
                    aspectRatioDropdown.value = 2;
                    return;
                case "21:9":
                    aspectRatioDropdown.value = 3;
                    return;
                case "32:9":
                    aspectRatioDropdown.value = 4;
                    return;
            }
        }

        private string GetCurrentAspectRatio()
        {
            float screenWidth = (float)Screen.width;
            float screenHeight = (float)Screen.height;
            float aspectRatio = screenWidth / screenHeight;

            int gcd = GCD(Screen.width, Screen.height);
            int aspectRatioWidth = (int)(screenWidth / gcd);
            int aspectRatioHeight = (int)(screenHeight / gcd);

            return aspectRatioWidth + ":" + aspectRatioHeight;
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }




        public Dictionary<string, List<Vector2Int>> availableScreenResolutions;

        private void Awake()
        {
            availableScreenResolutions = new Dictionary<string, List<Vector2Int>>
            {
                {
                    "4:3", new List<Vector2Int>
                    {
                        new Vector2Int(640, 480),
                        new Vector2Int(800, 600),
                        new Vector2Int(1024, 768),
                        new Vector2Int(1152, 864),
                        new Vector2Int(1280, 960),
                        new Vector2Int(1400, 1050),
                        new Vector2Int(1600, 1200)
                    }
                },
                {
                    "16:9", new List<Vector2Int>
                    {
                        new Vector2Int(1280, 720),
                        new Vector2Int(1366, 768),
                        new Vector2Int(1600, 900),
                        new Vector2Int(1920, 1080),
                        new Vector2Int(2560, 1440),
                        new Vector2Int(3840, 2160),
                        new Vector2Int(7680, 4320)
                    }
                },
                {
                    "16:10", new List<Vector2Int>
                    {
                        new Vector2Int(960, 600),
                        new Vector2Int(1280, 800),
                        new Vector2Int(1440, 900),
                        new Vector2Int(1680, 1050),
                        new Vector2Int(1920, 1200),
                        new Vector2Int(2560, 1600),
                        new Vector2Int(3840, 2400)
                    }
                },
                {
                    "21:9", new List<Vector2Int>
                    {
                        new Vector2Int(1280, 540),
                        new Vector2Int(1680, 720),
                        new Vector2Int(1920, 810),
                        new Vector2Int(2560, 1080),
                        new Vector2Int(3440, 1440),
                        new Vector2Int(3840, 1600),
                        new Vector2Int(5120, 2160)
                    }
                },
                {
                    "32:9", new List<Vector2Int>
                    {
                        new Vector2Int(2560, 720),
                        new Vector2Int(3840, 1080),
                        new Vector2Int(5120, 1440),
                        new Vector2Int(6880, 1944),
                        new Vector2Int(7680, 2160),
                        new Vector2Int(8640, 2430),
                        new Vector2Int(10240, 2880)
                    }
                }
            };
        }


        public void PickAspectRatio(int aspectType)
        {
            switch (aspectType)
            {
                case 0:
                    SetAspectRatio("16:9");
                    return;
                case 1:
                    SetAspectRatio("16:10");
                    return;
                case 2:
                    SetAspectRatio("4:3");
                    return;
                case 3:
                    SetAspectRatio("21:9");
                    return;
                case 4:
                    SetAspectRatio("32:9");
                    return;

            }
        }

        void SetAspectRatio(string selectedAspectRatio)
        {
            List<Vector2Int> resolutionsForAspectRatio = GetAvailableResolutionsForAspectRatio(selectedAspectRatio);

            // Clear and update the resolution dropdown with the new list of resolutions
            resolutionDropdown.options.Clear();
            foreach (Vector2Int resolution in resolutionsForAspectRatio)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.x + "x" + resolution.y));
            }
            resolutionDropdown.RefreshShownValue();


            currentScreenAspect = selectedAspectRatio;

            for(int i = 0; i < availableScreenResolutions[selectedAspectRatio].Count; i++)
            {
                Vector2Int screenRes = new Vector2Int(Screen.width, Screen.height);
                if(screenRes == availableScreenResolutions[selectedAspectRatio][i])
                {
                    SetResolution(i, screenRes);
                    return;
                }
            }
            //jos nykyistä resoluutiota ei ole, tehdään näin
            Vector2Int newScreenRes = availableScreenResolutions[selectedAspectRatio][0];
            SetResolution(0, newScreenRes);
        }


        void SetResolution(int i, Vector2Int screenRes)
        {
            resolutionDropdown.value = i;
            Screen.SetResolution(screenRes.x, screenRes.y, Screen.fullScreenMode);
        }

        /// <summary>
        /// tämä otetaan canvasissa dropdownissa...
        /// </summary>
        /// <param name="i"></param>
        public void PickResolution(int i)
        {
            Vector2Int newScreenRes = availableScreenResolutions[currentScreenAspect][i];

            SetResolution(i, newScreenRes);
        }


        /// <summary>
        /// otetaan resoluutiot aspect ration perusteella...
        /// </summary>
        /// <param name="aspectRatio"></param>
        /// <returns></returns>
        public List<Vector2Int> GetAvailableResolutionsForAspectRatio(string aspectRatio)
        {
            if (availableScreenResolutions.ContainsKey(aspectRatio))
            {
                return availableScreenResolutions[aspectRatio];
            }
            else
            {
                Debug.LogError("Invalid aspect ratio provided.");
                return null;
            }
        }

    }
}

